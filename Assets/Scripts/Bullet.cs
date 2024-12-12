using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private int damage = 10; // Bullet Damage
    [SerializeField] private float speed = 10f;  // Bullet speed
    [SerializeField] private float lifetime = 5f; // Time before the bullet disappears

    private Rigidbody2D rb;

    private void Start()
    {
        // Initialize Rigidbody2D and set velocity
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = transform.up * speed; // Move the bullet in the "up" direction of its transform
        }

        // Destroy the bullet after a certain time
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the bullet collides with a building
        if (collision.gameObject.layer == LayerMask.NameToLayer("Building"))
        {
            Debug.Log("Bullet hit a building");
            Destroy(gameObject); // Destroy bullet on impact with a building
        }
        // Check if the bullet collides with an enemy
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            // Get the EnemyAI (or EnemyHealth) component
            var enemyAI = collision.gameObject.GetComponent<EnemyAI>();

            if (enemyAI != null)
            {
                // Apply damage to the enemy
                enemyAI.TakeDamage(damage);
            }

            // Destroy the bullet after hitting the enemy
            Destroy(gameObject);
        }
        // Optional: Handle other collisions
        else
        {
            Destroy(gameObject); // Destroy the bullet on any other collision
        }
    }
}
