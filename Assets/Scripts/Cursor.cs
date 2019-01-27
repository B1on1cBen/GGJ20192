using UnityEngine;
using Medley.Extensions;

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
    public Furniture possesedFurniture = null;

    [SerializeField] GhostMover ghost;
    [SerializeField] string rotateLeftButton;
    [SerializeField] string rotateRightButton;
    [SerializeField] float rotateSpeed = 10;
    [SerializeField] float moveInterval = .75f;
    [SerializeField] float moveSpeed = 10;
    [SerializeField] AudioSource turnSource;
    [SerializeField] AudioSource moveSource;
    [SerializeField] AudioSource cantMoveSource;
    [SerializeField] AudioSource possessSource;
    [SerializeField] AudioSource furnitureSource;
    [SerializeField] AudioClip[] furnitureSounds;
    [SerializeField] AudioClip turnSound;
    [SerializeField] AudioClip moveSound;
    [SerializeField] AudioClip cantMoveSound;
    [SerializeField] AudioClip possessSound;
    [SerializeField] AudioClip unpossessSound;
    [SerializeField] Material gridMaterial;
    [SerializeField] Material selectedGridMaterial;

    float wantedYRot;
    float moveTimer;
    int previousH;
    int previousV;

    Vector3 wantedPosition;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        currentBlock = gameManager.topLeft.GetComponent<BuildingBlock>();
        currentBlock.GetComponent<Renderer>().material = selectedGridMaterial;
        transform.position = currentBlock.transform.position;
        wantedPosition = transform.position;
    }

    void Update()
    {
        Rotate();
        Move();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (possesedFurniture == null)
            {
                if (currentBlock.Occupant != null)
                {
                    ghost.Possess();
                    possessSource.PlayOneShot(possessSound);
                    possesedFurniture = currentBlock.Occupant.GetComponent<Furniture>();
                }
                else
                {
                    cantMoveSource.PlayOneShot(cantMoveSound);
                }
            }
            else
            {
                ghost.Unpossess();
                possessSource.PlayOneShot(unpossessSound);
                possesedFurniture = null;
            }
        }
    }

    private void Move()
    {
        int h = (int)Input.GetAxisRaw("Horizontal");
        int v = (int)Input.GetAxisRaw("Vertical");

        if ((h != 0 || v != 0) && (previousH != h || previousV != v))
        {
            if (possesedFurniture == null)
                moveSource.PlayOneShot(moveSound);

            int newH = h;
            int newV = v;

            int rotY = (int)Quaternion.Euler(new Vector3(0, wantedYRot, 0)).eulerAngles.y;

            //Debug.Log("Rot Y: " + rotY);

            if (rotY >= 85 && rotY <= 95)
                rotY = 90;

            if (rotY >= 175 && rotY <= 185)
                rotY = 180;

            if (rotY >= 265 && rotY <= 275)
                rotY = 270;

            if (rotY >= 0 && rotY <= 5)
                rotY = 0;

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
            ghost.MoveToPoint(wantedPosition);

            if (wantedPosition != currentBlock.transform.position)
            {           
                if (possesedFurniture != null)
                {
                    Direction dir = Direction.North;
                    if(newV == 1)
                    {
                        dir = Direction.South;
                    } else if(newH == 1)
                    {
                        dir = Direction.East;
                    } else if(newV == -1)
                    {
                        dir = Direction.North;
                    } else if(newH == -1)
                    {
                        dir = Direction.West;
                    }
                    print(dir);
                    gameManager.GetComponent<WinningScript>().winCheck();
                    if(gameManager.canMove(possesedFurniture, dir))
                    {
                        print("moving");
                        furnitureSource.Stop();
                        furnitureSource.PlayRandomClip(furnitureSounds);
                        gameManager.move(possesedFurniture, dir);
                        currentBlock.GetComponent<Renderer>().material = gridMaterial;
                        currentBlock = possesedFurniture.OriginSquare.GetComponent<BuildingBlock>();
                        currentBlock.GetComponent<Renderer>().material = selectedGridMaterial;
                    } else
                    {
                        print("blocked");
                        cantMoveSource.PlayOneShot(cantMoveSound);
                    }
                } else
                {
                    currentBlock.GetComponent<Renderer>().material = gridMaterial;
                    currentBlock = currentBlock.GetNeighbor(newH, -newV);
                    currentBlock.GetComponent<Renderer>().material = selectedGridMaterial;
                }
            }
        }

        transform.position = Vector3.Lerp(transform.position, wantedPosition, Time.deltaTime * moveSpeed);

        previousH = h;
        previousV = v;
    }

    void Rotate()
    {
        if (Input.GetButtonDown(rotateLeftButton))
        {
            wantedYRot += 90;
            turnSource.PlayOneShot(turnSound);
        }

        if (Input.GetButtonDown(rotateRightButton))
        {
            wantedYRot -= 90;
            turnSource.PlayOneShot(turnSound);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles.x, wantedYRot, 0), rotateSpeed * Time.deltaTime);
    }
}
