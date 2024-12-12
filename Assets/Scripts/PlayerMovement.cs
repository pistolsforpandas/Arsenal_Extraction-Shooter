using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 7f;
    [SerializeField] private float acceleration = 0.1f;
    private Rigidbody2D rb;
    private PlayerHealth PlayerHealth;
    private Vector2 direction;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        PlayerHealth = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (PlayerHealth.isDead == false)
            {
            // Handle player movement input
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");
            direction = new Vector2(moveX, moveY).normalized;

            // Rotate the player to face the mouse
            RotatePlayerToMouse();
            }
        else
            {
                return;
            }
    }

    void FixedUpdate()
    {
        // Apply movement
        Vector2 targetVelocity = direction * speed;
        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, targetVelocity, acceleration);
    }

    void RotatePlayerToMouse()
    {
        // Get the mouse position in world space
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calculate the direction from the player to the mouse
        Vector2 directionToMouse = mousePosition - rb.position;

        // Calculate the angle between the player and the mouse
        float angle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;

        // Apply the angle to the player's rotation
        rb.rotation = angle - 90;
    }
}
