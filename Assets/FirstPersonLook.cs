using UnityEngine;

public class FirstPersonLook : MonoBehaviour
{
    public float sensitivity = 1.5f;
    private Vector2 lookInput;
    private float pitch = 0f;

    private PlayerControls controls;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake()
    {
        controls = new PlayerControls();
        controls.Camera.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        controls.Camera.Look.canceled += _ => lookInput = Vector2.zero;
    }

    void OnEnable()
    {
        controls.Enable();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnDisable()
    {
        controls.Disable();
    }

    void LateUpdate()
    {
        float mouseX = lookInput.x * sensitivity;
        float mouseY = lookInput.y * sensitivity;

        transform.parent.Rotate(0f, mouseX, 0f); 

        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -60f, 60f);
        transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }
}
