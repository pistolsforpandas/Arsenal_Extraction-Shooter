using UnityEngine;

public class RaycastGun : MonoBehaviour
{
    [Header("Gun Properties")]
    [SerializeField] private int damage = 10; // Damage dealt by the gun
    [SerializeField] private float range = 20f; // Maximum range of the raycast
    [SerializeField] private LayerMask hitLayers; // Layers to interact with (e.g., enemies, buildings)

    [Header("Visuals")]
    [SerializeField] private LineRenderer lineRenderer; // Optional: For visualizing the shot
    [SerializeField] private float lineDuration = 0.1f; // How long the line renderer stays visible

    [Header("Audio")]
    [SerializeField] private AudioClip shootSound;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Shoot()
    {
        // Play shooting sound
        if (shootSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(shootSound);
        }

        // Perform the raycast
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, range, hitLayers);

        if (hit.collider != null)
        {
            Debug.Log($"Hit {hit.collider.name}");

            // Check if it's an enemy
            if (hit.collider.CompareTag("Enemy"))
            {
                var enemyAI = hit.collider.GetComponent<EnemyAI>();
                if (enemyAI != null)
                {
                    enemyAI.TakeDamage(damage); // Apply damage
                }
            }

            // Check if it's a building
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Building"))
            {
                Debug.Log("Hit a building");
            }

            // Show the visual effect (line renderer)
            if (lineRenderer != null)
            {
                StartCoroutine(ShowLine(hit.point));
            }
        }
        else
        {
            // If no hit, show the ray extending to max range
            if (lineRenderer != null)
            {
                StartCoroutine(ShowLine(transform.position + transform.up * range));
            }
        }
    }

    private System.Collections.IEnumerator ShowLine(Vector3 endPoint)
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, endPoint);
        lineRenderer.enabled = true;

        yield return new WaitForSeconds(lineDuration);

        lineRenderer.enabled = false;
    }
}
