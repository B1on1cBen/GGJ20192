using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerationScript : MonoBehaviour {

    public GameObject GenericBuildingBlock;

    public void generate(int x, int y)
    {
        GameObject currentBlock = Instantiate(GenericBuildingBlock);
        GameManager.manager.topLeft = currentBlock;
        GameObject topLeft = currentBlock;

        for (int i = 0; i < x; i++)
        {
            currentBlock = topLeft;

            while (currentBlock.GetComponent<BuildingBlock>().Borders[(int)Direction.East] != null)
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

        GameManager.manager.topLeft = GameManager.manager.topLeft.GetComponent<BuildingBlock>().Borders[(int)Direction.East];

        currentBlock = GameManager.manager.topLeft;
        for(int i = 0; i < x; i++)
        {
            GameObject next = currentBlock.GetComponent<BuildingBlock>().Borders[(int)Direction.East];
            if (currentBlock.GetComponent<BuildingBlock>().Borders[(int)Direction.East] != null)
            {
                while (next.GetComponent<BuildingBlock>().Borders[(int)Direction.South] != null)
                {
                    currentBlock = currentBlock.GetComponent<BuildingBlock>().Borders[(int)Direction.South];
                    next = next.GetComponent<BuildingBlock>().Borders[(int)Direction.South];
                    print(currentBlock.name);
                    currentBlock.GetComponent<BuildingBlock>().Borders[(int)Direction.East] = next;
                    next.GetComponent<BuildingBlock>().Borders[(int)Direction.West] = currentBlock;
                }

                while (currentBlock.GetComponent<BuildingBlock>().Borders[(int)Direction.North] != null)
                {
                    currentBlock = currentBlock.GetComponent<BuildingBlock>().Borders[(int)Direction.North];
                }

                currentBlock = currentBlock.GetComponent<BuildingBlock>().Borders[(int)Direction.East];
            }
        }

        GenericBuildingBlock.SetActive(false);
        GameManager.manager.topLeft = topLeft.GetComponent<BuildingBlock>().Borders[1];
        Destroy(topLeft.gameObject);

        foreach (Furniture furniture in GameManager.manager.furniture)
        {
            currentBlock = GameManager.manager.topLeft;

            for (int i = 1; i < furniture.gameObject.GetComponent<SpawningOrigin>().x; i++)
            {
                currentBlock = currentBlock.GetComponent<BuildingBlock>().Borders[(int)Direction.East];
            }
            for (int i = 1; i < furniture.gameObject.GetComponent<SpawningOrigin>().y; i++)
            {
                currentBlock = currentBlock.GetComponent<BuildingBlock>().Borders[(int)Direction.South];
            }

            furniture.gameObject.transform.position = new Vector3(currentBlock.transform.position.x, furniture.gameObject.transform.position.y, currentBlock.transform.position.z);
            furniture.gameObject.transform.RotateAround(furniture.gameObject.transform.position, Vector3.up, 90 * (int)furniture.facing);

            BuildingBlock originBlock = currentBlock.GetComponent<BuildingBlock>();
            originBlock.Occupant = furniture.gameObject;
            GameObject topBlock = originBlock.gameObject;
            furniture.OriginSquare = topBlock;

            for (x = 0; x < furniture.width; x++)
            {
                for (y = 0; y < furniture.length; y++)
                {
                    currentBlock.GetComponent<BuildingBlock>().Occupant = furniture.gameObject;
                    currentBlock = currentBlock.GetComponent<BuildingBlock>().Borders[(int)furniture.facing];
                }
                topBlock = topBlock.GetComponent<BuildingBlock>().Borders[(int)furniture.facing];
                currentBlock = topBlock;
            }
        }
    }
}
