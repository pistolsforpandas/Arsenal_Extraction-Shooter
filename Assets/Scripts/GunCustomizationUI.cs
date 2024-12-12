using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GunCustomizationUI : MonoBehaviour
{
    public TextMeshProUGUI gunNameText;
    public TMP_Dropdown barrelDropdown;
    public TMP_Dropdown scopeDropdown;
    public TMP_Dropdown UnderbarrelDropdown;
    public TMP_Dropdown StockDropdown;
    public TMP_Dropdown MagazineDropdown;
    public Image attachmentPreviewImage;

    private Gun currentGun;



    public void Initialize(Gun gun)
    {
        currentGun = gun;
    gunNameText.text = gun.gunName;

    PopulateDropdown(barrelDropdown, gun.GetAvailableAttachments(AttachmentType.Barrel));
    PopulateDropdown(scopeDropdown, gun.GetAvailableAttachments(AttachmentType.Scope));
    PopulateDropdown(UnderbarrelDropdown, gun.GetAvailableAttachments(AttachmentType.Underbarrel));
    PopulateDropdown(StockDropdown, gun.GetAvailableAttachments(AttachmentType.Stock));
    PopulateDropdown(MagazineDropdown, gun.GetAvailableAttachments(AttachmentType.Magazine));
    }


    private void PopulateDropdown(TMP_Dropdown dropdown, List<Attachment> attachments)
    {
        dropdown.options.Clear();
        foreach (var attachment in attachments)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData(attachment.attachmentName));
        }
    }
    public void OnApplyAttachment()
    {
       // Get the selected attachment for each type
    Attachment selectedBarrel = currentGun.GetAttachmentByName(barrelDropdown.options[barrelDropdown.value].text);
    Attachment selectedScope = currentGun.GetAttachmentByName(scopeDropdown.options[scopeDropdown.value].text);

    // Apply these attachments to the gun
    currentGun.ApplyAttachment(selectedBarrel);
    currentGun.ApplyAttachment(selectedScope);

    // Update preview image or other UI elements if necessary
    UpdateAttachmentPreview();
    }

    private void UpdateAttachmentPreview()
    {
        // Example: Update the preview image with the current gun's attachment
        attachmentPreviewImage.sprite = currentGun.GetPreviewSprite();
    }

    public void OnRemoveAttachment()
    {
        currentGun.RemoveAllAttachments();
        Initialize(currentGun); // Refresh the UI
    }
}
