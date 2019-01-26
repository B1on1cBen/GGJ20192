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

    public bool canMove(Furniture block, Direction dir)
    {
        //how many spots to check
        int count = block.width;
        //if (block.facing == Direction.North || block.facing == Direction.South)
        //{
        //    count = block.width;
        //} else if(block.facing == Direction.East || block.facing == Direction.West)
        //{
        //    count = block.length;
        //}


        //move to border
        BuildingBlock startBlock = block.OriginSquare.GetComponent<BuildingBlock>();
        while(startBlock.Borders[(int)dir] != null && startBlock.Borders[(int)dir].GetComponent<BuildingBlock>().Occupant == block.gameObject)
        {
            startBlock = startBlock.Borders[(int)dir].GetComponent<BuildingBlock>();
        }
        int set = (int)dir - 1;
        set = (set == -1) ? 3 : set;
        while (startBlock.Borders[set] != null && startBlock.Borders[set].GetComponent<BuildingBlock>().Occupant == block.gameObject)
        {
            startBlock = startBlock.Borders[(int)Direction.North].GetComponent<BuildingBlock>();
        }

        if (startBlock.Borders[(int)dir] != null)
        {
            BuildingBlock check = startBlock.Borders[(int)dir].GetComponent<BuildingBlock>();
            do
            {
                count--;
                if (check.Occupant != null)
                {
                    print("exiting");
                    return false;
                    
                }

                if (check.Borders[((int)dir + 1) % 4] == null)
                {
                    if (count != 0)
                    {
                        print("exiting");
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
            print("exiting");
            return false;
            

        }
        //if (startBlock.Borders[(int)dir] != null)
        //{
        //    //BuildingBlock check = startBlock.Borders[(int)dir].GetComponent<BuildingBlock>();
        //    int set = ((int)dir + 1) % 4;
        //    for (int i = 0; i < count; i++)
        //    {
        //        if (check.Occupant != null || (check.Borders[set] == null && i + 1 < count))
        //        {
        //            if (check.Occupant != null)
        //            {
        //                print("exiting");
        //            } else if(check.Borders[set] == null)
        //            {
        //                print("exiting");

        //            }
        //            return false;
        //        }
        //        else
        //        {
        //            if (check.Borders[set] != null)
        //            {
        //                check = check.Borders[set].GetComponent<BuildingBlock>();
        //            }
        //        }
        //    }
        //} else
        //{
        //    print("exiting");
        //    return false;
        //}

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
