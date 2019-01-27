using UnityEngine;
using UnityEngine.UI;

public class ImageScript : MonoBehaviour
{
    bool active = false;
    public Image img;

    private void Start()
    {
            img.gameObject.SetActive(active);
    }

    void Update () {
        if (Input.GetButtonDown("TogglePhoto"))
        {
            active = !active;
            img.gameObject.SetActive(active);
        }
	}

}
