using UnityEngine;
public class InventoryGrid : MonoBehaviour
{
    [Header("Grid Properties")]
    [SerializeField] private int gridWidth = 10;
    [SerializeField] private int gridHeight = 10;
    [SerializeField] private float maxWeight = 50f;
    public int GridWidth => gridWidth;
    public int GridHeight => gridHeight;
    private ItemBase[,] grid;
    private float currentWeight;

    void Awake()
    {
        grid = new ItemBase[gridWidth, gridHeight];
    }

    public bool CanPlaceItem(ItemBase item, int x, int y)
    {
        // Check if placement exceeds grid boundaries
        if (x + item.size.width > gridWidth || y + item.size.height > gridHeight)
        {
            Debug.Log($"Cannot place {item.itemName}: Position ({x},{y}) would exceed grid bounds");
            return false;
        }

        // Check if space is occupied
        for (int i = x; i < x + item.size.width; i++)
        {
            for (int j = y; j < y + item.size.height; j++)
            {
                if (grid[i, j] != null)
                {
                    Debug.Log($"Cannot place {item.itemName}: Position ({i},{j}) is occupied by {grid[i,j].itemName}");
                    return false;
                }
            }
        }

        // Check weight limit
        if (currentWeight + item.weight > maxWeight)
        {
            Debug.Log($"Cannot place {item.itemName}: Would exceed weight limit");
            return false;
        }

        Debug.Log($"Position ({x},{y}) is valid for {item.itemName} with size {item.size.width}x{item.size.height}");
        return true;
    }

    public bool PlaceItem(ItemBase item, int x, int y)
    {
        if (!CanPlaceItem(item, x, y))
            return false;

        // Clear any existing item in this space
        ItemBase existingItem = grid[x, y];
        if (existingItem != null)
        {
            RemoveItem(x, y);
        }

        // Place item in grid
        for (int i = x; i < x + item.size.width; i++)
        {
            for (int j = y; j < y + item.size.height; j++)
            {
                grid[i, j] = item;
            }
        }

        currentWeight += item.weight;
        return true;
    }

    public ItemBase GetItemAt(int x, int y)
    {
        if (x >= 0 && x < gridWidth && y >= 0 && y < gridHeight)
        {
            return grid[x, y];
        }
        return null;
    }
    public ItemBase RemoveItem(int x, int y)
    {
        ItemBase item = grid[x, y];
        if (item == null)
            return null;

        // Clear all grid spaces occupied by item
        for (int i = x; i < x + item.size.width; i++)
        {
            for (int j = y; j < y + item.size.height; j++)
            {
                grid[i, j] = null;
            }
        }

        currentWeight -= item.weight;
        return item;
    }


    void Start()
    {
        // Create a test item
        GameObject testObj = new GameObject("TestItem");
        ItemBase testItem = testObj.AddComponent<ItemBase>();
        testItem.itemName = "Test Item";
        testItem.weight = 1f;
        testItem.size = new ItemSize { width = 2, height = 2 };

        // Try to place the test item
        bool placed = PlaceItem(testItem, 0, 0);
        Debug.Log($"Test item placed: {placed}");
    }
        void OnGUI()
    {
        // Draw a debug window in the top-left corner
        GUILayout.BeginArea(new Rect(10, 10, 200, 400));
        GUILayout.Label("Inventory Grid Debug");
        
        // Display grid contents
        for (int y = 0; y < gridHeight; y++)
        {
            GUILayout.BeginHorizontal();
            for (int x = 0; x < gridWidth; x++)
            {
                string cellContent = (grid[x, y] != null) ? "■" : "□";
                GUILayout.Label(cellContent, GUILayout.Width(20));
            }
            GUILayout.EndHorizontal();
        }
        
        // Display current weight
        GUILayout.Label($"Current Weight: {currentWeight}/{maxWeight}");
        GUILayout.EndArea();
    }
}
