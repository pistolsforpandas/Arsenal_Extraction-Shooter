using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private Transform shootPoint;     // Position where the raycast originates
    [SerializeField] private float fireRate = 0.5f;    // Time between shots
    [SerializeField] private float alertRadius = 10f;  // Radius to alert enemies
    [SerializeField] private LayerMask enemyLayer;     // Layer mask to detect enemies
    [SerializeField] private LayerMask hitLayers;      // Layers that the raycast can hit (e.g., enemies, buildings)
    [SerializeField] private float range = 20f;        // Maximum range of the raycast
    [SerializeField] private int damage = 10;          // Damage dealt by the shot
    [SerializeField] private LineRenderer lineRenderer; // Optional: For visualizing the shot
    [SerializeField] private float lineDuration = 0.1f; // Duration the shot trail is visible

    private PlayerHealth PlayerHealth;

    private float nextFireTime = 0f;


    void Start()
    {
        PlayerHealth = GetComponent<PlayerHealth>();
    }
    void Update()
    {
        if(PlayerHealth.isDead == false)
        {
            // Fire a raycast when the player presses the fire button (left mouse button)
            if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
            {
                Fire();
                nextFireTime = Time.time + fireRate;  // Prevent rapid firing (delay between shots)
            }
        }
    }

    void Fire()
    {
        // Perform the raycast
        RaycastHit2D hit = Physics2D.Raycast(shootPoint.position, shootPoint.up, range, hitLayers);

        if (hit.collider != null)
        {
            Debug.Log($"Hit {hit.collider.name}");

            // Check if the hit object is an enemy
            if (hit.collider.CompareTag("Enemy"))
            {
                EnemyAI enemyAI = hit.collider.GetComponent<EnemyAI>();
                if (enemyAI != null)
                {
                    enemyAI.TakeDamage(damage);  // Apply damage to the enemy
                }
            }

            // Handle visual feedback for the shot (optional)
            if (lineRenderer != null)
            {
                StartCoroutine(ShowShotTrail(hit.point));
            }
        }
        else
        {
            // If no object was hit, visualize the ray extending to max range
            if (lineRenderer != null)
            {
                StartCoroutine(ShowShotTrail(shootPoint.position + shootPoint.up * range));
            }
        }

        // Alert enemies within the specified radius
        AlertEnemies();
    }

    void AlertEnemies()
    {
        // Detect all enemies within the alert radius
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, alertRadius, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            // Call a method on the enemy's AI script to start chasing the player
            EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
            if (enemyAI != null)
            {
                enemyAI.Alert();
            }
        }
    }

    // Draw the alert radius in the editor for debugging
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, alertRadius);
    }

    // Show the shot trail using the line renderer (optional)
    private System.Collections.IEnumerator ShowShotTrail(Vector3 endPoint)
    {
        lineRenderer.SetPosition(0, shootPoint.position);
        lineRenderer.SetPosition(1, endPoint);
        lineRenderer.enabled = true;

        yield return new WaitForSeconds(lineDuration);

        lineRenderer.enabled = false;
    }
}