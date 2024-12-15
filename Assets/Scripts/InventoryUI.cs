using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private bool showDebugInfo = true;

    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private Transform inventoryItemContainer;
    [SerializeField] private GameObject inventoryItemPrefab;
    

    private InventoryGrid playerInventory;

    void Start()
    {
        PlayerController playerController = FindFirstObjectByType<PlayerController>();
        if (playerController != null)
        {
            playerInventory = playerController.inventory;
        }
        else
        {
            Debug.LogError("PlayerController not found.");
            return;
        }

        // Update the inventory UI initially
        UpdateInventoryUI();
    }

    public void UpdateInventoryUI()
    {
        if (playerInventory == null)
        {
            Debug.LogError("Player inventory is null!");
            return;
        }

        Debug.Log($"Updating inventory UI. Grid size: {playerInventory.GridWidth}x{playerInventory.GridHeight}");

        // Clear existing inventory items
        foreach (Transform child in inventoryItemContainer) 
        {
            Destroy(child.gameObject);
        }
        
        // Keep track of items we've already created UI elements for
        HashSet<ItemBase> processedItems = new HashSet<ItemBase>();

        // Create visual representation for each item
        for (int x = 0; x < playerInventory.GridWidth; x++)
        {
            for (int y = 0; y < playerInventory.GridHeight; y++)
            {
                ItemBase item = playerInventory.GetItemAt(x, y);
                if (item != null && !processedItems.Contains(item))
                {
                    Debug.Log($"Creating UI element for item: {item.itemName} at position ({x}, {y})");
                    GameObject itemUI = Instantiate(inventoryItemPrefab, inventoryItemContainer);
                    // Set position and other properties...
                    
                    processedItems.Add(item);
                }
            }
        }
    }

    private void EquipAttachment(Attachment item, Gun currentGun) 
    {
        // Remove existing attachment of the same type
        currentGun.RemoveAttachmentsOfType(item.attachmentType);

        // Apply the selected attachment
        currentGun.ApplyAttachment(item);

        // Update the UI
        UpdateInventoryUI(); 

        // Notify GunCustomizationUI to update its display
        GunCustomizationUI gunCustomizationUI = FindFirstObjectByType<GunCustomizationUI>(); 
        if (gunCustomizationUI != null) 
        {
            gunCustomizationUI.UpdateGunStats(); 
            gunCustomizationUI.UpdateAttachmentPreview(); 
        }
}
}