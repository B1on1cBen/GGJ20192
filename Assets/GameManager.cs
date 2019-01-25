using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager manager;

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
       // GetComponent<RoomGenerationScript>().generate(2, 2);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void generate()
    {
        GetComponent<RoomGenerationScript>().generate(2, 2);
    }
}

public enum Direction
{
    North = 0,
    East,
    West,
    South
}
