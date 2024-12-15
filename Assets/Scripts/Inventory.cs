using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public List<Attachment> items = new List<Attachment>();
    public int maxSize = 10; // Example: Maximum inventory size

    public void AddItem(Attachment item)
    {
        if (items.Count < maxSize)
        {
            items.Add(item);
            // Optionally: Update the inventory UI
        }
        else
        {
            Debug.Log("Inventory Full");
            // Handle inventory full condition (e.g., display a message)
        }
    }

    public void RemoveItem(Attachment item)
    {
        items.Remove(item);
        // Optionally: Update the inventory UI
    }

    public bool HasItem(Attachment item)
    {
        return items.Contains(item);
    }

    public List<Attachment> GetItemsOfType(AttachmentType type)
    {
        return items.FindAll(a => a.attachmentType == type);
    }
}