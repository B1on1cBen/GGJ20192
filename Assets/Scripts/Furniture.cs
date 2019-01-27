using UnityEngine;

public class Furniture : MonoBehaviour
{
    public Direction facing = Direction.North;

    public int length = 1;
    public int width = 1;

    public int WinningX;
    public int WinningY;

    public GameObject OriginSquare;
    public BuildingBlock WinningPosition;

    GameManager gameManager;

    void Start ()
    {
        gameManager = FindObjectOfType<GameManager>();
        WinningPosition = gameManager.topLeft.GetComponent<BuildingBlock>();
        for (int i = 0; i < WinningX; i++)
        {
            WinningPosition = WinningPosition.Borders[(int)Direction.East].GetComponent<BuildingBlock>();
        }
        for (int i = 0; i < WinningY; i++)
        {
            WinningPosition = WinningPosition.Borders[(int)Direction.South].GetComponent<BuildingBlock>();
        }
    }
}
