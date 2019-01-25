using Medley.Extensions;
using UnityEngine;

public class PlatformerController : MonoBehaviour
{
    public static readonly int GroundLayerMask = (1 << 10);

    [HideInInspector] public bool flipped;
    [HideInInspector] public Vector2 inputDir;
    [HideInInspector] public Rigidbody2D rb;
    [SerializeField] protected new Collider2D collider;

    // Saved so we can invert X to flip unit
    protected Vector3 defaultScale;

    // Default is right
    protected Vector2 direction = Vector2.right;

    [Range(0, 10)]
    [SerializeField] protected float groundCheckDist;

    [Range(0, 10)]
    [SerializeField] protected float moveSpeed = 4;
    protected Collider2D[] overlapResults = new Collider2D[1];
    protected RaycastHit2D[] raycastResults = new RaycastHit2D[1];

    private readonly float skinWidth = 0.05f;
    [SerializeField] private float currentGravity = -56.25f;
    [SerializeField] private float hangTimeVelocity;
    private BoxCollider2D hitBox;
    [SerializeField] private float hitboxOffsetYLand = -0.03101492f;
    [SerializeField] private float hitboxSizeYLand = 1.319766f;
    [SerializeField] private float jumpVelocity = -550;
    [SerializeField] private PhysicsMaterial2D landMat;
    private bool onSlope; // Updated by GetMoveVector(). Informs when gravity should be turned on or off.
    private RayCastOrigins rayCastOrigins;
    [Range(0, .5f), SerializeField] private float slopeCheckDistOffset = 0.01f;
    [Range(0, .5f), SerializeField] private float stickDeadzone = 0.1f;

    public bool OnGround
    {
        get
        {
            if (OnSlope)
                return true;

            if (Physics2D.RaycastNonAlloc(rayCastOrigins.BottomLeft - Vector2.right * skinWidth, Vector2.down,
                                          raycastResults, groundCheckDist, GroundLayerMask) != 0)
                return true;

            if (Physics2D.RaycastNonAlloc(rayCastOrigins.Bottom, Vector2.down, raycastResults, groundCheckDist,
                                          GroundLayerMask) != 0)
                return true;

            return Physics2D.RaycastNonAlloc(rayCastOrigins.BottomRight + Vector2.right * skinWidth, Vector2.down,
                                             raycastResults, groundCheckDist, GroundLayerMask) != 0;
        }
    }

    private bool IsJumpHeld
    {
        get { return Input.GetButton("Jump") || Input.GetButtonDown("Jump"); }
    }

    private bool IsJumpPressed
    {
        get { return Input.GetButtonDown("Jump"); }
    }

    private bool MovementInputPressed
    {
        get { return Mathf.Abs(inputDir.x) > stickDeadzone || Mathf.Abs(inputDir.y) > stickDeadzone; }
    }

    private bool OnSlope
    {
        get
        {
            var hit = Physics2D.Raycast(transform.position, Vector2.down,
                                        collider.bounds.size.y + skinWidth + slopeCheckDistOffset, GroundLayerMask);

            if (!hit)
                return false;

            if (hit.normal == Vector2.up)
                return false;

            // Assumes that player collider is perfect square
            return hit.distance <= collider.bounds.size.y + skinWidth + slopeCheckDistOffset;
        }
    }

    public Bounds GetColliderBox()
    {
        if (!collider)
            return new Bounds();

        if (collider.GetType() == typeof(BoxCollider2D))
        {
            BoxCollider2D box = (BoxCollider2D)collider;
            return box.bounds;
        }

        if (collider.GetType() == typeof(CircleCollider2D))
        {
            CircleCollider2D circle = (CircleCollider2D)collider;
            return circle.CircleBox();
        }

        if (collider.GetType() == typeof(CapsuleCollider2D))
        {
            CapsuleCollider2D capsule = (CapsuleCollider2D)collider;
            return capsule.CapsuleBox();
        }

        return new Bounds();
    }

    private void Start()
    {
        if (!collider)
            collider = GetComponent<Collider2D>();

        rb = GetComponent<Rigidbody2D>();
        hitBox = GetComponent<BoxCollider2D>();
        defaultScale = transform.localScale;
        UpdateCollisionRays();
    }

    private void AddGravity()
    {
        rb.AddForce(new Vector2(0, currentGravity * rb.mass));
    }

    private Vector2 GetMovementVector()
    {
        if (inputDir.x == 0) return Vector2.zero;
        // We can check for slopes to the side of us.

        Vector2 raycastOrigin, rayDirection;

        if (Mathf.Sign(inputDir.x) == 1)
        {
            raycastOrigin = rayCastOrigins.BottomRight;
            rayDirection = Vector2.right;
        }
        else
        {
            raycastOrigin = rayCastOrigins.BottomLeft;
            rayDirection = Vector2.left;
        }

        var hit = Physics2D.Raycast(raycastOrigin, rayDirection, 10, GroundLayerMask);

        if (!hit) return GetMovingDownSlopeVector();
        // There is some sort of wall at some distance. Could be a slope, not sure

        if (hit.distance - skinWidth > skinWidth + Mathf.Abs(inputDir.x * moveSpeed))
            return GetMovingDownSlopeVector();

        //Now we know that the wall or slope is next to us.

        if (Mathf.Abs(hit.normal.x) != 1.0f && hit.normal.y != 0.0f)
        {
            //There is a slope to the side of us.

            onSlope = true;

            var angle = Vector2.Angle(hit.normal, Vector2.up);

            var min = angle <= 90 - angle ? angle : 90 - angle;
            var max = Mathf.Abs(90 - min);

            float hUpSlope, vUpSlope;

            if (angle <= 45)
            {
                hUpSlope = max / 90;
                vUpSlope = min / 90;
            }
            else
            {
                hUpSlope = min / 90;
                vUpSlope = max / 90;
            }

            if (Mathf.Sign(inputDir.x) == -1)
                hUpSlope *= -1;

            var slopeMoveDir = new Vector2(hUpSlope, vUpSlope).normalized;

            return slopeMoveDir * moveSpeed;
        }

        // If obstacle is not a slope, then it must be a wall, so stop.
        // Also move closer to the wall if possible
        var distToWall = hit.distance - skinWidth;

        return distToWall <= skinWidth ? Vector2.zero : new Vector2(distToWall * Mathf.Sign(inputDir.x), 0);

        // Did not see a wall within 10 meters. Checking down slopes...
    }

    private Vector2 GetMovingDownSlopeVector()
    {
        var raycastDirection = Vector2.down;
        Vector2 raycastOrigin;

        if (Mathf.Sign(inputDir.x) == 1)
            raycastOrigin = rayCastOrigins.BottomLeft - Vector2.right * (inputDir.x * moveSpeed);
        else
            raycastOrigin = rayCastOrigins.BottomRight - Vector2.right * (inputDir.x * moveSpeed);

        var hit = Physics2D.Raycast(raycastOrigin, raycastDirection, 10, GroundLayerMask);

        if (!hit) return inputDir * moveSpeed;
        // There is something below us at some distance. Could be floor, could be slope, hey I DONT KNOW

        if (hit.distance - skinWidth > skinWidth + Mathf.Abs(inputDir.x * moveSpeed))
            return inputDir * moveSpeed;

        // Now we know we are on top of something

        if (hit.normal.y == 1.0f) return inputDir * moveSpeed;
        // Now we know we are on a slope moving down

        var slopeDownVector = inputDir * moveSpeed -
                              Vector2.up * (Mathf.Sign(inputDir.x) * inputDir.x * moveSpeed);

        // Check if we are going to run into a floor or not
        raycastDirection = Vector2.down;

        if (Mathf.Sign(inputDir.x) != 1)
            raycastOrigin = rayCastOrigins.BottomLeft - Vector2.right * (inputDir.x * moveSpeed);
        else
            raycastOrigin = rayCastOrigins.BottomRight - Vector2.right * (inputDir.x * moveSpeed);

        hit = Physics2D.Raycast(raycastOrigin, raycastDirection, 10, GroundLayerMask);

        if (!hit) return slopeDownVector;
        if (hit.distance - skinWidth < Mathf.Abs(slopeDownVector.y))
            slopeDownVector.y -= slopeDownVector.y - (hit.distance - skinWidth);

        return slopeDownVector;
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
    }

    private void LateUpdate()
    {
        onSlope = false;
    }

    private void Move()
    {
        // Clamps vertical movement depending on in water or not
        inputDir.y = 0;

        transform.Translate(GetMovementVector(), Space.World);

        // Flips player horizontally
        if (!(Mathf.Abs(inputDir.x) > stickDeadzone))
            return;

        if (Mathf.Sign(transform.localScale.x) != Mathf.Sign(inputDir.x))
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    private void Update()
    {
#if UNITY_ANDROID
        inputDir = new Vector2(UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetAxisRaw("Horizontal"),
        					   UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetAxisRaw("Vertical"));
#else
        inputDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
#endif

        // This must be called every frame to ensure performance
        UpdateCollisionRays();

        if (MovementInputPressed)
            Move();

        if (IsJumpPressed && OnGround)
            Jump();

        UpdateGravity();
        inputDir.y = 0;

        if (!onSlope || !OnGround)
            AddGravity();
    }

    private void UpdateCollisionRays()
    {
        var bounds = GetColliderBox();
        bounds.Expand(skinWidth * -2);

        rayCastOrigins.BottomLeft = new Vector3(bounds.min.x, bounds.min.y);
        rayCastOrigins.Bottom = new Vector3(bounds.center.x, bounds.min.y);
        rayCastOrigins.BottomRight = new Vector3(bounds.max.x, bounds.min.y);
    }

    private void UpdateGravity()
    {
        var velocity = rb.velocity;

        if (velocity.y > 0 && !IsJumpHeld)
        {
            if (velocity.y > hangTimeVelocity)
                velocity = Vector2.up * hangTimeVelocity;
        }

        rb.velocity = velocity;
    }

    private struct RayCastOrigins
    {
        public Vector2 BottomLeft, Bottom, BottomRight;
    }
}