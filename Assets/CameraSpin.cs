using UnityEngine;

public class CameraSpin : MonoBehaviour
{
    public float rotateSpeed;

    private void Update()
    {
        transform.Rotate(0, rotateSpeed, 0);
    }
}
