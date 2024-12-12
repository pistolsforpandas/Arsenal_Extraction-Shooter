using UnityEngine;

public enum AttachmentType
{
    Barrel,
    Scope,
    Underbarrel,
    Stock,
    Magazine
}

public class Attachment : MonoBehaviour
{
    public string attachmentName;
    public AttachmentType attachmentType;

    // Stat modifiers
    public float damageModifier;
    public float fireRateModifier;
    public float accuracyModifier;

    // Optional: Add an attachment preview image if needed
    public Sprite previewImage;
}
