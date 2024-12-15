using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gun : MonoBehaviour
{

        [Header("Shooting")]
    [SerializeField] private Transform shootPoint;
    [SerializeField] public float fireRate = 0.5f;
    [SerializeField] private float alertRadius = 10f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask hitLayers;
    [SerializeField] public float range = 20f;
    [SerializeField] public int damage = 10;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float lineDuration = 0.1f;
    
    
    private float nextFireTime = 0f;
    public string gunName;
    public float baseDamage;
    public float accuracy;

    private Sprite defaultSprite;

    // A list of current attachments on the gun
    public List<Attachment> currentAttachments = new List<Attachment>();

    // A reference to all possible attachments (this can be loaded from a database or assigned in the editor)
    public List<Attachment> allAttachments;

    public void Fire()
    {
         if (Time.time >= nextFireTime)
        {
            RaycastHit2D hit = Physics2D.Raycast(shootPoint.position, shootPoint.up, range, hitLayers);

            if (hit.collider != null)
            {
                Debug.Log($"Hit {hit.collider.name}");

                if (hit.collider.CompareTag("Enemy"))
                {
                    EnemyAI enemyAI = hit.collider.GetComponent<EnemyAI>();
                    if (enemyAI != null)
                    {
                        enemyAI.TakeDamage(damage);
                    }
                }

                if (lineRenderer != null)
                {
                    StartCoroutine(ShowShotTrail(hit.point));
                }
            }
            else
            {
                if (lineRenderer != null)
                {
                    StartCoroutine(ShowShotTrail(shootPoint.position + shootPoint.up * range));
                }
            }

            AlertEnemies();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void AlertEnemies()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, alertRadius, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
            if (enemyAI != null)
            {
                enemyAI.Alert();
            }
        }
    }

    public Sprite GetPreviewSprite()
    {
        // Implement logic to determine the preview sprite based on current attachments
        // For example, you could have a dictionary mapping attachment combinations to sprites
        // or use a more complex algorithm to generate a preview dynamically

        // Placeholder: Return a default sprite for now
        return defaultSprite;
    }
    public void ApplyAttachment(Attachment attachment)
    {
        if (attachment == null || currentAttachments.Contains(attachment))
            return;

        // Modify gun stats based on attachment
        
        damage += attachment.damageModifier;
        fireRate += attachment.fireRateModifier;
        accuracy += attachment.accuracyModifier;

        // Add the attachment to the current list
        currentAttachments.Add(attachment);
        Debug.Log("Selected attachments" + currentAttachments);
    }

    public void RemoveAttachment(Attachment attachment)
    {
        if (attachment == null || !currentAttachments.Contains(attachment))
            return;

        // Revert gun stats
        damage -= attachment.damageModifier;
        fireRate -= attachment.fireRateModifier;
        accuracy -= attachment.accuracyModifier;

        // Remove the attachment from the current list
        currentAttachments.Remove(attachment);
    }

    public void RemoveAttachmentsOfType(AttachmentType type)
    {
        List<Attachment> toRemove = currentAttachments.FindAll(a => a.attachmentType == type);
        foreach (var attachment in toRemove)
        {
            RemoveAttachment(attachment);
        }
    }

    // Fetch available attachments of a specific type
    public List<Attachment> GetAvailableAttachments(AttachmentType type)
    {
        return allAttachments.FindAll(a => a.attachmentType == type);
    }

    // Get an attachment by name (for use with UI dropdowns)
    public Attachment GetAttachmentByName(string name)
    {
        return allAttachments.Find(a => a.attachmentName == name);
    }

    // Remove all attachments (for reset functionality)
    public void RemoveAllAttachments()
    {
        foreach (var attachment in currentAttachments)
        {
            RemoveAttachment(attachment);
        }
    }

   private IEnumerator ShowShotTrail(Vector3 endPoint)
{
    lineRenderer.SetPosition(0, shootPoint.position);
    lineRenderer.SetPosition(1, endPoint);
    lineRenderer.enabled = true;

    yield return new WaitForSeconds(lineDuration);

    lineRenderer.enabled = false;
}
}
