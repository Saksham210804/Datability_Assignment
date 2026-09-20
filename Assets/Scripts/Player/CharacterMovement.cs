using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 8f;

    [Header("Ground Detection")]
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody rb;
    private Vector3 movementDirection;
    private bool isFrozen;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        // Stay still until the match actually starts (GameManager calls Activate()).
        rb.isKinematic = true;
    }

    private void FixedUpdate()
    {
        if (isFrozen || rb.isKinematic)
            return;

        ApplyMovement();
    }

    public void Activate()
    {
        rb.isKinematic = false;
    }

    public void Move(Vector3 direction)
    {
        if (isFrozen)
            return;

        direction.y = 0f;

        if (direction.sqrMagnitude > 1f)
        {
            direction.Normalize();
        }

        movementDirection = direction;
    }

    public void StopMoving()
    {
        movementDirection = Vector3.zero;
    }

    public void Freeze()
    {
        isFrozen = true;
        movementDirection = Vector3.zero;

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
        }
    }

    private void ApplyMovement()
    {
        Vector3 targetVelocity = movementDirection * moveSpeed;

        rb.linearVelocity = new Vector3(
            targetVelocity.x,
            rb.linearVelocity.y,
            targetVelocity.z
        );

        // Rotation is fully controlled below, so cancel any spin from collisions.
        rb.angularVelocity = Vector3.zero;

        RotateTowards(movementDirection);
    }

    public void Jump()
    {
        if (isFrozen)
            return;

        if (!IsGrounded())
            return;

        Vector3 velocity = rb.linearVelocity;
        velocity.y = jumpForce;

        rb.linearVelocity = velocity;
    }

    public bool IsGrounded()
    {
        return Physics.Raycast(
            transform.position,
            Vector3.down,
            groundCheckDistance,
            groundLayer
        );
    }

    public bool IsMovingUp()
    {
        return rb.linearVelocity.y > 0.1f;
    }

    public float GetVerticalVelocity()
    {
        return rb.linearVelocity.y;
    }

    private void RotateTowards(Vector3 direction)
    {
        if (direction.sqrMagnitude < 0.01f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        rb.MoveRotation(Quaternion.Slerp(
            rb.rotation,
            targetRotation,
            rotationSpeed * Time.fixedDeltaTime
        ));
    }
}
