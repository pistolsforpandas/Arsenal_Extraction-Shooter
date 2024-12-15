using UnityEngine;

public enum AttachmentType
{
    Barrel,
    Scope,
    Underbarrel,
    Stock,
    Magazine
}

public class Attachment : ItemBase // Changed from MonoBehaviour
{

    public string attachmentName;
    public AttachmentType attachmentType;
    public int damageModifier;
    public float fireRateModifier;
    public float accuracyModifier;
    


    // Optional: Add an attachment preview image if needed
    public Sprite previewImage;
}
