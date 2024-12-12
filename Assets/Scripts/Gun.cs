using UnityEngine;
using System.Collections.Generic;

public class Gun : MonoBehaviour
{
    public string gunName;
    public float baseDamage;
    public float fireRate;
    public float accuracy;

    // A list of current attachments on the gun
    public List<Attachment> currentAttachments = new List<Attachment>();

    // A reference to all possible attachments (this can be loaded from a database or assigned in the editor)
    public List<Attachment> allAttachments;

    public void ApplyAttachment(Attachment attachment)
    {
        if (attachment == null || currentAttachments.Contains(attachment))
            return;

        // Modify gun stats based on attachment
        baseDamage += attachment.damageModifier;
        fireRate += attachment.fireRateModifier;
        accuracy += attachment.accuracyModifier;

        // Add the attachment to the current list
        currentAttachments.Add(attachment);
    }

    public void RemoveAttachment(Attachment attachment)
    {
        if (attachment == null || !currentAttachments.Contains(attachment))
            return;

        // Revert gun stats
        baseDamage -= attachment.damageModifier;
        fireRate -= attachment.fireRateModifier;
        accuracy -= attachment.accuracyModifier;

        // Remove the attachment from the current list
        currentAttachments.Remove(attachment);
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
}
