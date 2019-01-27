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
        //int count = 1;
        //if(dir == Direction.North || dir == Direction.South)
        //{
        //    count = block.length;
        //} else
        //{
        //    count = block.width;
        //}

        //move to border
        BuildingBlock startBlock = block.OriginSquare.GetComponent<BuildingBlock>();
        List<BuildingBlock> lst = new List<BuildingBlock>();
        List<BuildingBlock> subLst = new List<BuildingBlock>();
        BuildingBlock currentBlock = startBlock;
        for(int i = 0; i < block.width; i++)
        {
            lst.Add(currentBlock);
            if (currentBlock.Borders[(int)Direction.East] != null)
            {
                currentBlock = currentBlock.Borders[(int)Direction.East].GetComponent<BuildingBlock>();
            }
        }

        foreach (BuildingBlock top in lst)
        {
            currentBlock = top;
            for (int i = 1; i < block.length; i++)
            {
                if (currentBlock.Borders[(int)Direction.South] != null)
                {
                    subLst.Add(currentBlock.Borders[(int)Direction.South].GetComponent<BuildingBlock>());
                    currentBlock = currentBlock.Borders[(int)Direction.South].GetComponent<BuildingBlock>();
                }
            }
        }

       foreach(BuildingBlock obj in subLst)
        {
            lst.Add(obj.GetComponent<BuildingBlock>());
        }

       foreach(BuildingBlock buildingBlock in lst)
        {
            if (buildingBlock.Borders[(int)dir] != null) {
                if (buildingBlock.Borders[(int)dir].GetComponent<BuildingBlock>().Occupant != null && buildingBlock.Borders[(int)dir].GetComponent<BuildingBlock>().Occupant != block.gameObject)
                {
                    print("exiting");
                    return false;
                }
            } else
            {
                print("exiting");
                return false;
            }
        }

       // while(startBlock.Borders[(int)dir] != null && startBlock.Borders[(int)dir].GetComponent<BuildingBlock>().Occupant == block.gameObject)
       // {
       //     startBlock = startBlock.Borders[(int)dir].GetComponent<BuildingBlock>();
       // }
       // int set = ((int)dir+1)%4;
       //// set = (set == -1) ? 3 : set;
       // while (startBlock.Borders[set] != null && startBlock.Borders[set].GetComponent<BuildingBlock>().Occupant == block.gameObject)
       // {
       //     startBlock = startBlock.Borders[set].GetComponent<BuildingBlock>();
       // }

       // if (startBlock.Borders[(int)dir] != null)
       // {
       //     BuildingBlock check = startBlock.Borders[(int)dir].GetComponent<BuildingBlock>();
       //     do
       //     {
       //         count--;
       //         if (check.Occupant != null)
       //         {
       //             //print("exiting");
       //             return false;
                    
       //         }

       //         if (check.Borders[((int)dir + 1) % 4] == null)
       //         {
       //             if (count != 0)
       //             {
       //                 //print("exiting");
       //                 return false;
                        
       //             }
       //         }
       //         else
       //         {
       //             check = check.Borders[((int)dir + 1) % 4].GetComponent<BuildingBlock>();
       //         }
                
       //     } while (count > 0);
       // } else
       // {
       //     //print("exiting");
       //     return false;
       // }

        return true;
    }

    public void move(Furniture block, Direction dir)
    {
        BuildingBlock movingTo = block.OriginSquare.GetComponent<BuildingBlock>().Borders[(int)dir].GetComponent<BuildingBlock>();
        BuildingBlock topBlock = block.OriginSquare.GetComponent<BuildingBlock>();

        BuildingBlock startBlock = block.OriginSquare.GetComponent<BuildingBlock>();
        List<BuildingBlock> lst = new List<BuildingBlock>();
        List<BuildingBlock> subLst = new List<BuildingBlock>();
        BuildingBlock currentBlock = startBlock;
        for (int i = 0; i < block.width; i++)
        {
            lst.Add(currentBlock);
            if (currentBlock.Borders[(int)Direction.East] != null)
            {
                currentBlock = currentBlock.Borders[(int)Direction.East].GetComponent<BuildingBlock>();
            }
        }

        foreach (BuildingBlock top in lst)
        {
            currentBlock = top;
            for (int i = 1; i < block.length; i++)
            {
                if (currentBlock.Borders[(int)Direction.South] != null)
                {
                    subLst.Add(currentBlock.Borders[(int)Direction.South].GetComponent<BuildingBlock>());
                    currentBlock = currentBlock.Borders[(int)Direction.South].GetComponent<BuildingBlock>();
                }
            }
        }

        foreach (BuildingBlock obj in subLst)
        {
            lst.Add(obj.GetComponent<BuildingBlock>());
        }

        foreach (BuildingBlock buildingBlock in lst)
        {
            buildingBlock.Occupant = null;
        }

        movingTo.Occupant = block.gameObject;
        block.OriginSquare = movingTo.gameObject;
        block.gameObject.transform.position = new Vector3(movingTo.transform.position.x, block.gameObject.transform.position.y, movingTo.transform.position.z);

        lst.Clear();
        subLst.Clear();

        startBlock = block.OriginSquare.GetComponent<BuildingBlock>();
        lst = new List<BuildingBlock>();
        lst.Add(movingTo);
        subLst = new List<BuildingBlock>();
        currentBlock = startBlock;
        for (int i = 1; i < block.width; i++)
        {
            lst.Add(currentBlock);
            if (currentBlock.Borders[(int)Direction.East] != null)
            {
                currentBlock = currentBlock.Borders[(int)Direction.East].GetComponent<BuildingBlock>();
            }
        }

        foreach (BuildingBlock top in lst)
        {
            currentBlock = top;
            for (int i = 1; i < block.length; i++)
            {
                if (currentBlock.Borders[(int)Direction.South] != null)
                {
                    subLst.Add(currentBlock.Borders[(int)Direction.South].GetComponent<BuildingBlock>());
                    currentBlock = currentBlock.Borders[(int)Direction.South].GetComponent<BuildingBlock>();
                }
            }
        }

        foreach (BuildingBlock obj in subLst)
        {
            lst.Add(obj.GetComponent<BuildingBlock>());
        }

        foreach (BuildingBlock buildingBlock in lst)
        {
            buildingBlock.Occupant = block.gameObject;
        }

        //block.gameObject.transform.RotateAround(movingTo.gameObject.transform.position, Vector3.up, 90 * (int)block.facing);
    }

    public bool canRotate(Furniture block, Direction dir)
    {
        print(block.gameObject.name);
        print(dir);
        BuildingBlock topBlock = block.OriginSquare.GetComponent<BuildingBlock>();
        BuildingBlock currentBlock = topBlock;
        for (int x = 0; x < block.width; x++)
        {
            print(x);
            for (int y = 1; y < block.length; y++)
            {
                currentBlock = currentBlock.GetComponent<BuildingBlock>().Borders[((int)block.facing + (int)dir + 1) % 4].GetComponent<BuildingBlock>();
            }

            if ((currentBlock.Occupant != null && currentBlock.Occupant != block.gameObject))
            {
                return false;
            }
            else if(topBlock.GetComponent<BuildingBlock>().Borders[((int)block.facing + (int)dir) % 4] == null)
            {
                return false;
            } else
            {
                topBlock = topBlock.GetComponent<BuildingBlock>().Borders[((int)block.facing + (int)dir) % 4].GetComponent<BuildingBlock>();
                currentBlock = topBlock;
            }
        }

        return true;
    }
}

public enum Direction
{
    North = 0,
    East,
    South,
    West   
}
