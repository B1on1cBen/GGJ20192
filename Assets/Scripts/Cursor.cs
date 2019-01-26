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

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.generate();
        currentBlock = gameManager.topLeft.GetComponent<BuildingBlock>();
        transform.position = currentBlock.transform.position;
    }

    void Update()
    {
        Rotate();
        Point movement = GetMovement();
        MoveToNewTile(movement);
    }

    void MoveToNewTile(Point movement)
    {
        if (movement.x == 0 && movement.y == 0)
            return;

        Vector3 wantedPosition = currentBlock.GetNeighborPosition(movement.x, movement.y);
        if (wantedPosition == currentBlock.transform.position)
            return;

        transform.position = Vector3.Lerp(transform.position, wantedPosition, Time.deltaTime * moveSpeed);
    }

    Point GetMovement()
    {
        if (moveTimer <= 0)
        {
            moveTimer = moveInterval;

            int h = (int)Input.GetAxisRaw("Horizontal");
            int v = (int)Input.GetAxisRaw("Vertical");

            return new Point(h, v);
        }
        else
        {
            moveTimer -= Time.deltaTime;
        }

        return new Point(0, 0);
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
