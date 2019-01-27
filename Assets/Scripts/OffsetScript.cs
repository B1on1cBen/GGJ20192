using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetScript : MonoBehaviour
{
    public float x = 0;
    public float y = 0;
    public float z = 0;

	// Use this for initialization
	void Start () {
        transform.position += new Vector3(x, y, z);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
