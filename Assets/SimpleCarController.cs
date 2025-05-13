using UnityEngine;
using UnityEngine.InputSystem;

public class SimpleCarController : MonoBehaviour
{
    public float maxSpeed = 20f;
    public float accelerationForce = 10f;
    public float turnSpeed = 50f;
    public float brakingForce = 15f;
    public float dragFactor = 0.99f;

    private float currentSpeed;
    private Vector2 moveInput;

    // Reference to the InputActionAsset-generated class
    private PlayerControls controls;

    private void Awake()
    {
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

    void Update()
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

        // Apply movement
        float move = currentSpeed * Time.deltaTime;
        float turn = moveInput.x * turnSpeed * Time.deltaTime * Mathf.Sign(currentSpeed);

        transform.Translate(0, 0, move);
        transform.Rotate(0, turn, 0);
    }
}
