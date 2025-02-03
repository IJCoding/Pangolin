using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private Transform target; // The target to follow (Pangolin)
    [SerializeField] private float distance = 5f; // Distance behind the target
    [SerializeField] private float height = 3f; // Height above the target
    [SerializeField] private float smoothSpeed = 0.125f; // Smoothness factor

    private Vector3 offset;

    void Start()
    {
        // Set the initial offset (position relative to the target)
        offset = new Vector3(0f, height, -distance);
    }

    void LateUpdate()
    {
        if (target != null)
        {
            // Calculate the desired position behind and above the target
            Vector3 desiredPosition = target.position + offset;

            // Smoothly move the camera to the desired position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Set the camera's position
            transform.position = smoothedPosition;

            // Make the camera look at the target (Pangolin)
            transform.LookAt(target);
        }
    }

    // Method to set the target of the camera (called from PangolinController)
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
