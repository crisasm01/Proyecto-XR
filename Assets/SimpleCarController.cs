using UnityEngine;
using UnityEngine.InputSystem;

public class SimpleCarController : MonoBehaviour
{
    [Header("Parámetros de movimiento")]
    public float maxSpeed = 20f;
    public float accelerationForce = 10f;
    public float turnSpeed = 50f;
    public float brakingForce = 15f;
    public float dragFactor = 0.95f;

    private float currentSpeed = 0f;
    private float steerInput = 0f;
    private float forwardInput = 0f;
    private float reverseInput = 0f;

    private PlayerControls controls;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        InitializeControls();
    }

    private void InitializeControls()
    {
        if (controls != null) return;

        controls = new PlayerControls();


        controls.Car.Steer.performed += ctx => steerInput = ctx.ReadValue<float>();
        controls.Car.Steer.canceled += ctx => steerInput = 0f;


        controls.Car.Reverse.performed += ctx => forwardInput = ctx.ReadValue<float>();
        controls.Car.Reverse.canceled += ctx => forwardInput = 0f;


        controls.Car.Forward.performed += ctx => reverseInput = ctx.ReadValue<float>();
        controls.Car.Forward.canceled += ctx => reverseInput = 0f;
    }

    private void OnEnable()
    {
        InitializeControls();
        controls.Car.Enable();
    }

    private void OnDisable()
    {
        if (controls != null)
            controls.Car.Disable();
    }

    private void FixedUpdate()
    {   


        float target = forwardInput - reverseInput;

        if (target > 0f)
        {
            currentSpeed = Mathf.MoveTowards(
                currentSpeed,
                maxSpeed * target,
                accelerationForce * Time.fixedDeltaTime
            );
        }
        else if (target < 0f)
        {
            currentSpeed = Mathf.MoveTowards(
                currentSpeed,
                maxSpeed * target,
                brakingForce * Time.fixedDeltaTime
            );
        }
        else
        {

            currentSpeed *= dragFactor;
        }

        Vector3 forwardVel = transform.forward * currentSpeed;
        rb.linearVelocity = new Vector3(
            forwardVel.x,
            rb.linearVelocity.y,
            forwardVel.z
        );


        if (Mathf.Abs(currentSpeed) > 0.1f)
        {

            float signedTurn = steerInput
                               * turnSpeed
                               * Time.fixedDeltaTime
                               * Mathf.Sign(currentSpeed);

            Quaternion turnOffset = Quaternion.Euler(0f, signedTurn, 0f);
            rb.MoveRotation(rb.rotation * turnOffset);
        }
    }
}
