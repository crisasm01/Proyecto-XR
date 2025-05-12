using UnityEngine;
using UnityEngine.InputSystem;

public class SimpleCarController : MonoBehaviour
{
    public float speed = 10f;
    public float turnSpeed = 50f;

    private Vector2 moveInput;

    // Reference to the InputActionAsset-generated class
    private PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();

        // Bind the Move action
        controls.Car.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Car.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    private void OnEnable()
    {
        controls.Car.Enable();
    }

    private void OnDisable()
    {
        controls.Car.Disable();
    }

    void Update()
    {
        float move = moveInput.y * speed * Time.deltaTime;
        float turn = moveInput.x * turnSpeed * Time.deltaTime;

        transform.Translate(0, 0, move);
        transform.Rotate(0, turn, 0);
    }
}
