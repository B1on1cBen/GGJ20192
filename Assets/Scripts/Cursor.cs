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
            wantedPosition = currentBlock.GetNeighborPosition(h, -v);

            if(wantedPosition != currentBlock.transform.position)
                currentBlock = currentBlock.GetNeighbor(h, -v);
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
