using UnityEngine;
public enum ItemType
{
    Weapon,
    Clothing,
    Attachment,
    Ammo,
    Magazine,
    Medical,
    Food,
    Tool,
    Material,
    Valuable
}

[System.Serializable]
public class ItemSize
{
    public int width = 1;
    public int height = 1;
}

public class ItemBase : MonoBehaviour
{
    [Header("Basic Properties")]
    public string itemName;
    public ItemType itemType;
    public string description;
    public Sprite itemSprite;

    [Header("Physical Properties")]
    public float weight;
    public float value;
    public ItemSize size;

    [Header("Stack Properties")]
    public bool isStackable;
    public int maxStackSize = 1;
    public int currentStackSize = 1;
}
