using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement3D : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 6f;
    public float acceleration = 20f;
    public float maxGroundAngle = 60f;

    [Header("Salto")]
    public float jumpForce = 7f;

    private Rigidbody rb;
    private Vector2 moveInput;
    private bool isGrounded;
    private Vector3 groundNormal = Vector3.up;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    // Movimiento
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    // Salto
    public void OnJump(InputValue value)
    {
        if (value.isPressed && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        UpdateGrounded();

        Vector3 inputDir = new Vector3(moveInput.x, 0f, moveInput.y);
        inputDir = Vector3.ClampMagnitude(inputDir, 1f);

        Vector3 moveDir = Vector3.ProjectOnPlane(inputDir, groundNormal).normalized;
        //float currentMaxSpeed = isGrounded ? moveSpeed : 0f;      (esta linea es para que no se pueda mover en el aire como habías pedido, pero no lo entiendo, porque entonces no puede subir a las plataformas)
        float currentMaxSpeed = moveSpeed;

        Vector3 targetVelocity = moveDir * currentMaxSpeed;

        Vector3 velocity = rb.linearVelocity;
        Vector3 velocityXZ = new Vector3(velocity.x, 0f, velocity.z);
        Vector3 newVelocityXZ = Vector3.MoveTowards(velocityXZ, targetVelocity, acceleration * Time.fixedDeltaTime);

        rb.linearVelocity = new Vector3(newVelocityXZ.x, velocity.y, newVelocityXZ.z);
    }

    void UpdateGrounded()
    {
        float rayLength = 1.1f;
        Vector3 origin = transform.position + Vector3.up * 0.1f;

        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, rayLength))
        {
            groundNormal = hit.normal;
            float groundAngle = Vector3.Angle(hit.normal, Vector3.up);
            isGrounded = groundAngle <= maxGroundAngle;
        }
        else
        {
            groundNormal = Vector3.up;
            isGrounded = false;
        }
    }


    [Header("Respawn")]
    public Transform respawnPoint;

    void Update()
    {
        if (transform.position.y < -5f) 
        {
            Respawn();
        }
    }

    public GameManager gameManager; 

    void Respawn()
    {
        rb.linearVelocity = Vector3.zero;
        transform.position = respawnPoint.position;

        
        if (gameManager != null)
        {
            gameManager.ResetPlatforms();
        }
    }
}