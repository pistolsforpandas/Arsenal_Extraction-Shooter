using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class GunCustomizationUI : MonoBehaviour
{
    [SerializeField] private GameObject InventoryPanel;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private InventoryUI inventoryUI; 

    public TextMeshProUGUI gunNameText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI fireRateText;
    public TextMeshProUGUI accuracyText;
    public Image attachmentPreviewImage;

    private Gun currentGun;

    void Start()
    {
        InventoryPanel.SetActive(false);

        if (playerController != null)
        {
            Initialize(playerController.playerGun);
        }
        else
        {
            Debug.LogError("PlayerController is not assigned!");
        }
    }

    public void Initialize(Gun gun)
    {
        currentGun = gun;
        gunNameText.text = gun.gunName;

        // Update UI with initial gun stats
        UpdateGunStats(); 
    }

    public void UpdateGunStats()
    {
        damageText.text = "Damage: " + currentGun.baseDamage.ToString();
        fireRateText.text = "Fire Rate: " + currentGun.fireRate.ToString();
        accuracyText.text = "Accuracy: " + currentGun.accuracy.ToString();
    }

    public void UpdateAttachmentPreview()
    {
        // Example: Update the preview image with the current gun's attachment
        attachmentPreviewImage.sprite = currentGun.GetPreviewSprite();
    }

    public void OnRemoveAttachment()
    {
        currentGun.RemoveAllAttachments();
        UpdateGunStats(); 
        UpdateAttachmentPreview(); 

        // Notify InventoryUI to update its display
        if (inventoryUI != null) 
        {
            inventoryUI.UpdateInventoryUI(); 
        }
    }
}