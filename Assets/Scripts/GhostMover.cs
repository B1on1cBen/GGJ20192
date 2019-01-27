using UnityEngine;

public class GhostMover : MonoBehaviour
{
    [SerializeField] float rotateSpeed;
    [SerializeField] float moveSpeed;
    [SerializeField] string possessTrigger;
    [SerializeField] string unpossessTrigger;
    [SerializeField] Animator animator;

    float yOffset;
    Vector3 wantedPoint;
    Quaternion wantedRotation;

    private void Awake()
    {
        yOffset = transform.position.y;
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, wantedPoint, Time.deltaTime * moveSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, wantedRotation, Time.deltaTime * rotateSpeed);
    }

    public void MoveToPoint(Vector3 point)
    {
        Vector3 modifiedPosition = transform.position;
        modifiedPosition.y = 0;

        Vector3 direction = Vector3.Normalize(point - modifiedPosition);
        wantedRotation = Quaternion.LookRotation(direction, Vector3.up);
        wantedPoint = point + Vector3.up * yOffset;
    }

    public void Possess()
    {
        animator.SetTrigger(possessTrigger);
    }

    public void Unpossess()
    {
        animator.SetTrigger(unpossessTrigger);
    }
}
