using UnityEngine;
using UnityEngine.UI;  // Import for UI elements like health bar

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;  // Maximum health of the player
    public int currentHealth;  // Current health
    [SerializeField] private Slider healthBar;  // Health bar UI element
    [SerializeField] private GameOverManager gameOverManager;

    public bool isDead = false;

    void Start()
    {
        // Initialize the player's health to max at the start
        currentHealth = maxHealth;

        // Update the health bar to reflect full health
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    // Method to handle taking damage
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Clamp health to prevent it from going below 0
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Update the health bar
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        // Check if the player is dead
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Method to handle player death
    void Die()
    {
        // Here you could trigger death animation, sound, etc.
        Debug.Log("Player has died!");
        isDead = true;
        gameOverManager.ShowGameOverScreen("Game Over");

        // Destroy the player game object or disable the player
        // Destroy(gameObject); // Uncomment if you want to destroy the player object
        // Or you could disable movement, for example:
        // this.enabled = false; // Disables the PlayerHealth script
    }
}
