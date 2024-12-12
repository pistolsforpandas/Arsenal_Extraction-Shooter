using UnityEngine;
using UnityEngine.SceneManagement;  // For scene loading
using TMPro;  // For TextMeshPro elements (if you're using TMP)

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;  // The panel showing the game over screen
    [SerializeField] private TMP_Text gameOverText;  // TextMeshPro component for displaying the game over text
    //[SerializeField] private float displayTime = 2f;  // Time to display the game over screen

    void Start()
    {
        gameOverPanel.SetActive(false);  // Hide the game over screen initially
    }

    // Call this method when the game ends (e.g., player dies)
    public void ShowGameOverScreen(string message)
    {
        gameOverPanel.SetActive(true);  // Show the game over panel
        gameOverText.text = message;   // Set the message (e.g., "Game Over")

        // Optionally, you can use a timer to automatically restart the game after a delay:
        // Invoke("RestartGame", displayTime);
    }

    // Restart the game (reload the current scene)
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // Reload the current scene
    }

    // Button handler for restart (this will be connected to the button in the UI)
    public void OnRestartButtonPressed()
    {
        RestartGame();
    }
}
