using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    [Header("Movement")]
    [SerializeField] private float speed = 3f;

    [Header("Combat")]
    [SerializeField] private int damage = 30;
    [SerializeField] private int scoreValue = 10;

    [Header("Detection")]
    [SerializeField] private float visionRange = 5f; // Vision distance
    [SerializeField] private float visionAngle = 90f; // Field of view angle
    [SerializeField] private float hearingRange = 3f; // Hearing radius
    [SerializeField] private float loseSightDelay = 2f; // Time to lose sight if player is out of vision

    [SerializeField] private LayerMask detectableLayers; // Include "Player" and "Building"

    private Transform player;
    private Rigidbody2D rb;

    private bool canSeePlayer = false;
    private bool canHearPlayer = false;
    private float loseSightTimer = 0f;

    private Vector2 lastKnownPosition;

    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (player != null)
        {
            // Check vision and hearing
            canSeePlayer = CheckVision();
            canHearPlayer = CheckHearing();

            if (canSeePlayer)
            {
                loseSightTimer = loseSightDelay; // Reset the lose sight timer
                lastKnownPosition = player.position; // Update last known position
                MoveTowardsPlayer(true); // Move towards player if they are seen
            }
            else if (loseSightTimer > 0)
            {
                loseSightTimer -= Time.deltaTime;
                MoveToLastKnownPosition(); // Move to last known position
            }
            // else
            // {
            //     StopMovement();
            // }
        }
    }

    public void Alert()
    {
        if (player != null)
        {
            Debug.Log($"Alerted");
            lastKnownPosition = player.position; // Store the player's last known position
            MoveToLastKnownPosition();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0) Die();
    }

    void MoveTowardsPlayer(bool isSeeingPlayer)
    {
        if (player != null)
        {
            // Get the direction towards the player
            Vector2 direction = (player.position - transform.position).normalized;

            // Rotate to face the player if seen
            if (isSeeingPlayer)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                rb.rotation = angle - 90f; // Aligns the sprite facing "up"
            }

            // Move towards the player using linearVelocity
            rb.linearVelocity = direction * speed;
        }
    }

    void MoveToLastKnownPosition()
    {
        // Get the direction towards the last known position
        Vector2 direction = (lastKnownPosition - (Vector2)transform.position).normalized;

        // Move towards the last known position
        rb.linearVelocity = direction * speed;

        // Rotate to face the movement direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.rotation = angle - 90f;

        // Stop chasing if close enough to the last known position
        if (Vector2.Distance(transform.position, lastKnownPosition) < 0.1f)
        {
            Debug.Log($"Stopping the chase");
            StopMovement();
        }
    }

    void StopMovement()
    {
        rb.linearVelocity = Vector2.zero; // Stop movement
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            playerHealth?.TakeDamage(damage);
        }
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} has died!");
        Destroy(gameObject);
        ScoreManager.Instance?.AddScore(scoreValue);
    }

    // Check if the player is within the cone of vision
    bool CheckVision()
    {
        if (player == null) return false;

        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Check distance
        if (distanceToPlayer > visionRange) return false;

        // Check angle
        float angle = Vector2.Angle(transform.up, directionToPlayer);
        if (angle > visionAngle / 2f) return false;

        // Adjust ray origin to avoid hitting self
        Vector2 rayOrigin = (Vector2)transform.position + directionToPlayer * 0.1f;

        // Check for obstacles using a raycast
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, directionToPlayer, visionRange, detectableLayers);
        Debug.DrawRay(rayOrigin, directionToPlayer * visionRange, Color.red, 0.1f);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player"))
            {
                return true; // Player is visible
            }
        }

        return false;
    }

    // Check if the player is within hearing range
    bool CheckHearing()
    {
        if (player == null) return false;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        return distanceToPlayer <= hearingRange;
    }

    // Visualize vision and hearing ranges in the Scene view
    void OnDrawGizmosSelected()
    {
        // Vision cone
        Gizmos.color = Color.yellow;
        Vector3 leftBoundary = Quaternion.Euler(0, 0, -visionAngle / 2) * transform.up * visionRange;
        Vector3 rightBoundary = Quaternion.Euler(0, 0, visionAngle / 2) * transform.up * visionRange;
        Gizmos.DrawRay(transform.position, leftBoundary);
        Gizmos.DrawRay(transform.position, rightBoundary);
        Gizmos.DrawWireSphere(transform.position, visionRange);

        // Hearing circle
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, hearingRange);
    }

    void OnDrawGizmos()
    {
        if (player != null)
        {
            Vector2 directionToPlayer = (player.position - transform.position).normalized;
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, directionToPlayer * visionRange);
        }
    }
}
