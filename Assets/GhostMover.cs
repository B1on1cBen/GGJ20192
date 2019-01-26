using UnityEngine;

public class GhostMover : MonoBehaviour
{
    public Transform cursor;

    private void LateUpdate()
    {
        transform.position = cursor.position;
    }
}
