using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furniture : MonoBehaviour {

    public GameObject OriginSquare;
    public Direction facing = Direction.North;

    public int length = 1;
    public int width = 1;

    public int WinningX;
    public int WinningY;

    public BuildingBlock WinningPosition;

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
     public void init()
    {
        WinningPosition = GameManager.manager.topLeft.GetComponent<BuildingBlock>();
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
