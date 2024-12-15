using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public int x;
    public int y;
    public ItemBase currentItem;
    
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI debugText;
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Vector2 originalPosition;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (currentItem == null) return;
        
        originalPosition = rectTransform.anchoredPosition;
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currentItem == null) return;
        
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (currentItem == null) return;
        
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        rectTransform.anchoredPosition = originalPosition;
    }

    public void OnDrop(PointerEventData eventData)
    {
        InventorySlot fromSlot = eventData.pointerDrag.GetComponent<InventorySlot>();
        if (fromSlot != null)
        {
            // Handle the item swap or movement logic here
            // This will need to coordinate with your InventoryGrid
        }
    }

    public void UpdateSlot(ItemBase item)
    {
        currentItem = item;
        if (item != null)
        {
            if (itemImage != null) itemImage.sprite = item.itemSprite;
            if (debugText != null) debugText.text = item.itemName;
        }
        else
        {
            if (itemImage != null) itemImage.sprite = null;
            if (debugText != null) debugText.text = "";
        }
    }
}
