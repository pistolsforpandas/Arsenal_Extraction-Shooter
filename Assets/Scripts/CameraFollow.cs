using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;    // Reference to the player's transform
    [SerializeField] private float smoothSpeed = 0.125f;   // Smoothness of the camera movement
    [SerializeField] private Vector3 offset;     // Default offset to maintain distance between camera and player
    [SerializeField] private float aimStretchFactor = 0.5f; // How far the camera moves toward the cursor (0-1)

    void Start()
    {
        // Optionally get the player object dynamically if not assigned in the Inspector
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        // Set initial camera offset (based on the player's position)
        offset = new Vector3(0, 0, -10); // Default offset, adjust as needed for your camera's setup
    }

    void LateUpdate()
    {
        // Get mouse position in world space
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0; // Keep the z-axis at 0 since we're in 2D

        // Calculate the direction from the player to the mouse
        Vector3 aimDirection = (mouseWorldPosition - player.position) * aimStretchFactor;

        // Define the desired position of the camera
        Vector3 desiredPosition = player.position + offset + aimDirection;

        // Smoothly move the camera towards the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Update the camera's position
        transform.position = smoothedPosition;
    }
}
