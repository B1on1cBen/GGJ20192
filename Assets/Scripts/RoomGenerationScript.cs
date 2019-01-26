using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerationScript : MonoBehaviour {

    public GameObject GenericBuildingBlock;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void generate(int x, int y)
    {
        GameObject currentBlock = Instantiate(GenericBuildingBlock);
        GameManager.manager.topLeft = currentBlock;
        GameObject topLeft = currentBlock;

        for(int i = 0; i < x; i++)
        {
            currentBlock = topLeft;

            while(currentBlock.GetComponent<BuildingBlock>().Borders[(int)Direction.East] != null)
            {
                currentBlock = currentBlock.GetComponent<BuildingBlock>().Borders[(int)Direction.East];
            }

            GameObject newBlock = Instantiate(GenericBuildingBlock);
            newBlock.transform.position = new Vector3(currentBlock.transform.position.x + newBlock.transform.localScale.x, currentBlock.transform.position.y, currentBlock.transform.position.z);
            currentBlock.GetComponent<BuildingBlock>().Borders[(int)Direction.East] = newBlock;
            newBlock.GetComponent<BuildingBlock>().Borders[(int)Direction.West] = currentBlock;
            currentBlock = newBlock;

            for (int j = 1; j < y; j++)
            {
                newBlock = Instantiate(GenericBuildingBlock);
                newBlock.transform.position = new Vector3(currentBlock.transform.position.x, currentBlock.transform.position.y, currentBlock.transform.position.z + newBlock.transform.localScale.z);
                currentBlock.GetComponent<BuildingBlock>().Borders[(int)Direction.South] = newBlock;
                newBlock.GetComponent<BuildingBlock>().Borders[(int)Direction.North] = currentBlock;
                currentBlock = newBlock;
            }
        }

        GenericBuildingBlock.SetActive(false);
        GameManager.manager.topLeft = topLeft.GetComponent<BuildingBlock>().Borders[1];
        Destroy(topLeft.gameObject);

        foreach (Furniture furniture in GameManager.manager.furniture)
        {
            currentBlock = GameManager.manager.topLeft;

            for (int i = 0; i < furniture.gameObject.GetComponent<SpawningOrigin>().x; i++)
            {
                currentBlock = currentBlock.GetComponent<BuildingBlock>().Borders[(int)Direction.East];
            }
            for (int i = 0; i < furniture.gameObject.GetComponent<SpawningOrigin>().y; i++)
            {
                currentBlock = currentBlock.GetComponent<BuildingBlock>().Borders[(int)Direction.South];
            }

            BuildingBlock originBlock = currentBlock.GetComponent<BuildingBlock>();
            originBlock.Occupant = furniture.gameObject;

            for (int i = 1; i <= furniture.length; i++)
            {
                currentBlock = currentBlock.GetComponent<BuildingBlock>().Borders[(/*(int)furniture.facing +*/ (int)Direction.East) % 4];
                currentBlock.GetComponent<BuildingBlock>().Occupant = furniture.gameObject;
                GameObject startingBlock = currentBlock;
                furniture.OriginSquare = startingBlock;
                for (int j = 1; j <= furniture.width; j++)
                {
                    currentBlock = currentBlock.GetComponent<BuildingBlock>().Borders[(/*(int)furniture.facing +*/ (int)Direction.South) % 4];
                    currentBlock.GetComponent<BuildingBlock>().Occupant = furniture.gameObject;
                }
                currentBlock = startingBlock;
            }
            //Destroy(furniture.gameObject.GetComponent<SpawningOrigin>());
        }
    }
}
