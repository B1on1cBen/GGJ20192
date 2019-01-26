using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageScript : MonoBehaviour {

    bool active = false;
    public Image img;

    private void Start()
    {
            img.gameObject.SetActive(active);
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            active = !active;
            img.gameObject.SetActive(active);
        }
	}

}
