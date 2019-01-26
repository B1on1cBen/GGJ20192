using UnityEngine;

public struct Point
{
    public int x;
    public int y;

    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}

public class Cursor : MonoBehaviour
{
    GameManager gameManager;
    BuildingBlock currentBlock;

    [SerializeField] string rotateLeftButton;
    [SerializeField] string rotateRightButton;
    [SerializeField] float rotateSpeed = 10;
    [SerializeField] float moveInterval = .75f;
    [SerializeField] float moveSpeed = 10;

    float wantedYRot;
    float moveTimer;
    int previousH;
    int previousV;

    Vector3 wantedPosition;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.generate();
        currentBlock = gameManager.topLeft.GetComponent<BuildingBlock>();
        transform.position = currentBlock.transform.position;
        wantedPosition = transform.position;
    }

    void Update()
    {
        Rotate();
        int h = (int)Input.GetAxisRaw("Horizontal");
        int v = (int)Input.GetAxisRaw("Vertical");

        if (h != 0 || v != 0)
        {
            int newH = h;
            int newV = v;

            int rotY = (int)Quaternion.Euler(new Vector3(0, wantedYRot, 0)).eulerAngles.y;

            switch (rotY)
            {
                case 90:
                    newH = v;
                    newV = -h;
                    break;

                case 180:
                    newH = -h;
                    newV = -v;
                    break;

                case 270:
                    newH = -v;
                    newV = h;
                    break;

                case 0:
                default:
                    break;              
            }

            wantedPosition = currentBlock.GetNeighborPosition(newH, -newV);

            if (wantedPosition != currentBlock.transform.position)
                currentBlock = currentBlock.GetNeighbor(newH, -newV);
        }

        transform.position = Vector3.Lerp(transform.position, wantedPosition, Time.deltaTime * moveSpeed);
    }

    void Rotate()
    {
        if (Input.GetButtonDown(rotateLeftButton))
        {
            wantedYRot += 90;
        }

        if (Input.GetButtonDown(rotateRightButton))
        {
            wantedYRot -= 90;
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles.x, wantedYRot, 0), rotateSpeed * Time.deltaTime);
    }
}
