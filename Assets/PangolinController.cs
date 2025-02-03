using UnityEngine;

public class Cooldown
{ 
    public float min = 0f;
    public float cur = 0f;
    public float max = 10f;
}


[RequireComponent(typeof(CharacterController))]
public class PangolinController : MonoBehaviour
{
    [Header("Movement")]

    [Range(1f, 20f)]
    [SerializeField]
    public float moveSpeed = 5f; // Walking speed

    [Range(1f, 20f)]
    [SerializeField]
    public float runSpeed = 15f; // Running speed

    [SerializeField]
    public bool isRunning = false;

    [SerializeField]
    public Cooldown Running = new Cooldown();

    public bool canRun() { return Running.cur >= Running.min + 0.5f; }

    [Header("Jumping")]
    [Range(1f, 4f)]
    [SerializeField]
    public float jumpHeight = 2f; // Jump height

    [Header("Physics")]
    public float gravity = -9.81f; // Gravity force

    [Header("Ground Detection")]
    public float groundDistance = 0.4f; // Ground check distance
    public LayerMask groundMask; // To check if the character is on the ground

    private Vector3 velocity; // Character velocity
    private bool isGrounded; // Check if the character is on the ground

    private CharacterController controller; // CharacterController reference

    // Reference to the CameraController (to set the camera target)
    public CameraController cameraController;

    private void Start()
    {
        controller = GetComponent<CharacterController>();

        // Ensure the camera target is set to the Pangolin
        if (cameraController != null)
        {
            cameraController.SetTarget(transform); // Set the camera's target to the Pangolin
        }

        Running.max = 10;
        Running.cur = Running.max;
    }

    private void Update()
    {
        // Check if the character is on the ground
        isGrounded = controller.isGrounded;

        isRunning = Input.GetAxis("Sprint") > 0;

        // Add this inside your Update method
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Reset the vertical velocity when grounded
        }

        velocity.y += gravity * Time.deltaTime; // Apply gravity

        // Get input for movement
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // Apply movement speed (run if shift is held)
        float speed = isRunning ? runSpeed : moveSpeed;

        // Move the character
        controller.Move(move * speed * Time.deltaTime);

        // Jumping logic
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); // Apply jump velocity
        }
        
        // Apply gravity
        velocity.y += gravity * Time.deltaTime;

        // Apply vertical velocity
        controller.Move(velocity * Time.deltaTime);
    }
}
