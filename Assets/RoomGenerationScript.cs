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
            while(topLeft.GetComponent<BuildingBlock>().Borders[(int)Direction.East] != null)
            {
                currentBlock = topLeft.GetComponent<BuildingBlock>().Borders[(int)Direction.East];
                Debug.Log("stepped east");
            }

            GameObject newBlock = Instantiate(GenericBuildingBlock);
            newBlock.transform.position = new Vector3(currentBlock.transform.position.x + newBlock.transform.localScale.x, currentBlock.transform.position.y, currentBlock.transform.position.z);
            currentBlock.GetComponent<BuildingBlock>().Borders[(int)Direction.East] = newBlock;
            currentBlock = newBlock;

            for (int j = 0; j < y; j++)
            {
                newBlock = Instantiate(GenericBuildingBlock);
                newBlock.transform.position = new Vector3(currentBlock.transform.position.x, currentBlock.transform.position.y, currentBlock.transform.position.z + newBlock.transform.localScale.z);
                currentBlock.GetComponent<BuildingBlock>().Borders[(int)Direction.South] = newBlock;
                currentBlock = newBlock;
            }
        }
    }
}
