using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossesionScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    public Cursor cursor;

    int previousH;
    int previousV;

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            cursor.GetComponent<Cursor>().enabled = true;
            Destroy(this);
        }
    }

    void move()
    {
        int h = (int)Input.GetAxisRaw("Horizontal");
        int v = (int)Input.GetAxisRaw("Vertical");

        if ((h != 0 || v != 0) && (previousH != h || previousV != v))
        {
            if(h > 1)
            {
                
            }
        }
    }
}
