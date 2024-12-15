using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 7f;
    [SerializeField] private float acceleration = 0.1f;

    [Header("Health")]
    [SerializeField] private int maxHealth = 100;
    public int currentHealth;
    [SerializeField] private Slider healthBar;
    [SerializeField] private GameOverManager gameOverManager;

     [Header("Damage Effect")]
    [SerializeField] private float flashDuration = 0.15f;
    [SerializeField] private Color flashColor = new Color(1f, 0f, 0f, 0.3f);
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private CameraFollow cameraFollow;

    [SerializeField] private Gun gun;

    private Rigidbody2D rb;
    public bool isDead = false;
    public bool canMove = true;
    
    private Vector2 direction;

    public static PlayerController Instance;
    public Gun playerGun;
    public InventoryGrid inventory; // Instead of public Inventory inventory

    void Awake()
    {
        Instance = this;
        playerGun = FindFirstObjectByType<Gun>(); // Or assign it directly in the Inspector
    }


    private void Start()
    {
        // Screen shake system
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        cameraFollow = Camera.main.transform.parent.GetComponent<CameraFollow>();
        // Health Bar
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
    {
        CreateTestItem();
    }
        if (canMove == true)
        {
            // Handle player movement input
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");
            direction = new Vector2(moveX, moveY).normalized;

            // Rotate the player to face the mouse
            RotatePlayerToMouse();
        }
        else
        {
                return;
        }

        if(canMove == true)
        {
            // Detect the fire button
            if (Input.GetButton("Fire1"))
            {
                gun.Fire();
            }
        }
       
    }

    private void CreateTestItem()
    {
        // Create a new game object for the test item
        GameObject testObj = new GameObject("TestItem");
        ItemBase testItem = testObj.AddComponent<ItemBase>();
        
        // Set random size between 1x1 and 3x3
        testItem.size = new ItemSize 
        { 
            width = Random.Range(1, 4),
            height = Random.Range(1, 4)
        };
        
        testItem.itemName = $"Test Item {testItem.size.width}x{testItem.size.height}";
        testItem.weight = Random.Range(0.1f, 5f);
        
        // Try to find an empty spot in the inventory
        for (int x = 0; x < inventory.GridWidth; x++)
        {
            for (int y = 0; y < inventory.GridHeight; y++)
            {
                if (inventory.CanPlaceItem(testItem, x, y))
                {
                    inventory.PlaceItem(testItem, x, y);
                    FindFirstObjectByType<InventoryUI>()?.UpdateInventoryUI();
                    Debug.Log($"Created test item: {testItem.itemName}");
                    return;
                }
            }
        }
        
        // If no space was found
        Debug.Log("No space in inventory for test item");
        Destroy(testObj);
    }

    void FixedUpdate()
    {
        // Apply movement
        Vector2 targetVelocity = direction * speed;
        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, targetVelocity, acceleration);
    }

    void RotatePlayerToMouse()
    {
        // Get the mouse position in world space
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calculate the direction from the player to the mouse
        Vector2 directionToMouse = mousePosition - rb.position;

        // Calculate the angle between the player and the mouse
        float angle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;

        // Apply the angle to the player's rotation
        rb.rotation = angle - 90;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Attachment"))
        {
            Debug.Log($"Player contacted attachment: {other.gameObject.name}");
            Attachment attachment = other.GetComponent<Attachment>();
            if (attachment != null)
            {
                Debug.Log($"Attempting to pick up attachment: {attachment.itemName}");
                Debug.Log($"Attachment size: {attachment.size.width}x{attachment.size.height}");
                
                bool itemPlaced = false;
                for (int x = 0; x < inventory.GridWidth && !itemPlaced; x++)
                {
                    for (int y = 0; y < inventory.GridHeight && !itemPlaced; y++)
                    {
                        if (inventory.CanPlaceItem(attachment, x, y))
                        {
                            inventory.PlaceItem(attachment, x, y);
                            FindFirstObjectByType<InventoryUI>()?.UpdateInventoryUI();
                            Destroy(other.gameObject);
                            Debug.Log($"Item placed at position ({x}, {y})");
                            itemPlaced = true;
                            break;
                        }
                    }
                }
                
                if (!itemPlaced)
                {
                    Debug.Log("Failed to find valid placement position in inventory");
                }
            }
            else
            {
                Debug.LogError("Object tagged as Attachment but missing Attachment component");
            }
        }
}



    public void TakeDamage(int damage)
    {
         if (!isDead)
        {
            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            if (healthBar != null)
            {
                healthBar.value = currentHealth;
            }

            // Add these lines to trigger effects
            StartCoroutine(DamageFlashEffect());
            if (cameraFollow != null)
            {
                cameraFollow.TriggerShake();
            }

            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }
    //Flash effect when damaged.
     private IEnumerator DamageFlashEffect()
    {
        spriteRenderer.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
    }

    private void Die()
    {
        // Handle player death, e.g., game over screen, respawn, etc.
        Debug.Log("Player has died!");
        isDead = true;
        canMove = false;
        gameOverManager.ShowGameOverScreen("Game Over");
    }
}