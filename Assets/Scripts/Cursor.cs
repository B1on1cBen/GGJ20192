using UnityEngine;

public class Cursor : MonoBehaviour
{
    [SerializeField] string rotateLeftButton;
    [SerializeField] string rotateRightButton;
    [SerializeField] float rotateSpeed = 1;

    float wantedYRot;

    void Update()
    {
        if (Input.GetButtonDown(rotateLeftButton))
            wantedYRot += 90;

        if (Input.GetButtonDown(rotateRightButton))
            wantedYRot -= 90;

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles.x, wantedYRot, 0), rotateSpeed * Time.deltaTime);
    }
}
