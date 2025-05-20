using UnityEngine;
using UnityEngine.InputSystem;

public class SimpleCarController : MonoBehaviour
{
    public float maxSpeed = 20f;
    public float accelerationForce = 10f;
    public float turnSpeed = 50f;
    public float brakingForce = 15f;
    public float dragFactor = 0.95f;

    private float currentSpeed;
    private Vector2 moveInput;

    // Reference to the InputActionAsset-generated class
    private PlayerControls controls;

    // Rigidbody used for physics
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        InitializeControls();
    }

    private void InitializeControls()
    {
        if (controls == null)
        {
            controls = new PlayerControls();

            // Bind the Move action
            controls.Car.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
            controls.Car.Move.canceled += ctx => moveInput = Vector2.zero;
        }
    }

    private void OnEnable()
    {
        InitializeControls();
        controls.Car.Enable();
    }

    private void OnDisable()
    {
        if (controls != null)
        {
            controls.Car.Disable();
        }
    }

    private void FixedUpdate()
    {   
        // Handle acceleration and deceleration
        float targetSpeed = moveInput.y;
        
        if (targetSpeed > 0)
        {
            // Accelerating forward
            currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed * targetSpeed, accelerationForce * Time.deltaTime);
        }
        else if (targetSpeed < 0)
        {
            // Braking/Reversing
            currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed * targetSpeed, brakingForce * Time.deltaTime);
        }
        else
        {
            // No input - apply drag
            currentSpeed *= dragFactor;
        }

        Vector3 forwardVelocity = transform.forward * currentSpeed;
        rb.linearVelocity = new Vector3(forwardVelocity.x, rb.linearVelocity.y, forwardVelocity.z);

        if (Mathf.Abs(currentSpeed) > 0.1f)
        {
            float turn = moveInput.x * turnSpeed * Time.fixedDeltaTime * Mathf.Sign(currentSpeed);
            Quaternion turnOffset = Quaternion.Euler(0f, turn, 0f);
            rb.MoveRotation(rb.rotation * turnOffset);
        }

    }
}
