using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager manager;

    public int xLength;
    public int yLength;

    public List<Furniture> furniture = new List<Furniture>();

    private void Awake()
    { 
        if(manager == null)
        {
            DontDestroyOnLoad(gameObject);
            manager = this;
        } else if(manager != this)
        {
            Destroy(gameObject);
        }
    }

    public GameObject topLeft;

    public void generate()
    {
        GetComponent<RoomGenerationScript>().generate(xLength, yLength);
    }

    public int ConvertXYToNeighborIndex(int x, int y)
    {
        if (y == 1)
            return 0;

        if (x == 1)
            return 1;

        if (y == -1)
            return 2;

        if (x == -1)
            return 3;

        return 0;
    }

    public bool canMove(Furniture block, Direction dir)
    {
        //how many spots to check
        int count = block.width;


        //move to border
        BuildingBlock startBlock = block.OriginSquare.GetComponent<BuildingBlock>();
        while(startBlock.Borders[(int)dir] != null && startBlock.Borders[(int)dir].GetComponent<BuildingBlock>().Occupant == block.gameObject)
        {
            startBlock = startBlock.Borders[(int)dir].GetComponent<BuildingBlock>();
        }
        int set = ((int)dir+1)%4;
       // set = (set == -1) ? 3 : set;
        while (startBlock.Borders[set] != null && startBlock.Borders[set].GetComponent<BuildingBlock>().Occupant == block.gameObject)
        {
            startBlock = startBlock.Borders[set].GetComponent<BuildingBlock>();
        }

        if (startBlock.Borders[(int)dir] != null)
        {
            BuildingBlock check = startBlock.Borders[(int)dir].GetComponent<BuildingBlock>();
            do
            {
                count--;
                if (check.Occupant != null)
                {
                    //print("exiting");
                    return false;
                    
                }

                if (check.Borders[((int)dir + 1) % 4] == null)
                {
                    if (count != 0)
                    {
                        //print("exiting");
                        return false;
                        
                    }
                }
                else
                {

                    check = check.Borders[((int)dir + 1) % 4].GetComponent<BuildingBlock>();
                }
                
            } while (count > 0);
        } else
        {
            //print("exiting");
            return false;
            

        }

        return true;
    }

    public void move(Furniture block, Direction dir)
    {
        BuildingBlock movingTo = block.OriginSquare.GetComponent<BuildingBlock>().Borders[(int)dir].GetComponent<BuildingBlock>();
        BuildingBlock topBlock = block.OriginSquare.GetComponent<BuildingBlock>();
        BuildingBlock currentBlock = topBlock;
        for (int x = 0; x < block.width; x++)
        {
            for (int y = 0; y < block.length; y++)
            {
                currentBlock.GetComponent<BuildingBlock>().Occupant = null;
                if (currentBlock.Borders[((int)block.facing + 1) % 4])
                {
                    currentBlock = currentBlock.Borders[((int)block.facing + 1) % 4].GetComponent<BuildingBlock>();
                    currentBlock.GetComponent<BuildingBlock>().Occupant = null;
                }
            }
            if (topBlock.Borders[(int)block.facing] != null)
            {
                topBlock = topBlock.Borders[(int)block.facing].GetComponent<BuildingBlock>();
            }
            currentBlock = topBlock;
        }

        topBlock = movingTo;
        currentBlock = topBlock;

        for (int x = 0; x < block.width; x++)
        {
            for (int y = 1; y < block.length; y++)
            {
                currentBlock.GetComponent<BuildingBlock>().Occupant = block.gameObject;
                currentBlock = currentBlock.GetComponent<BuildingBlock>().Borders[((int)block.facing + 1) % 4].GetComponent<BuildingBlock>();
                if (currentBlock != null)
                {
                    currentBlock.GetComponent<BuildingBlock>().Occupant = block.gameObject;
                }
            }
            if (topBlock.GetComponent<BuildingBlock>().Borders[(int)block.facing] != null)
            {
                topBlock = topBlock.GetComponent<BuildingBlock>().Borders[(int)block.facing].GetComponent<BuildingBlock>();
            }
            currentBlock = topBlock;
        }


        movingTo.Occupant = block.gameObject;
        block.OriginSquare = movingTo.gameObject;
        block.gameObject.transform.position = new Vector3(movingTo.transform.position.x, block.gameObject.transform.position.y, movingTo.transform.position.z);
        //block.gameObject.transform.RotateAround(movingTo.gameObject.transform.position, Vector3.up, 90 * (int)block.facing);
    }
}

public enum Direction
{
    North = 0,
    East,
    South,
    West   
}
