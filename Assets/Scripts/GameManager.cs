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

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void generate()
    {
        GetComponent<RoomGenerationScript>().generate(xLength, yLength);
    }
}

public enum Direction
{
    North = 0,
    East,
    West,
    South
}
