using UnityEngine;
using UnityEngine.UI;
using TMPro;  // For UI elements like the score display

public class ScoreManager : MonoBehaviour
{
    private int currentScore = 0;  // Tracks the current score
    [SerializeField] private TMP_Text scoreText;  // UI Text element to display the score

    public static ScoreManager Instance { get; private set; }

 void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        // Initialize the score display at the start
        UpdateScoreUI();
    }

    // Method to increase the score
    public void AddScore(int scoreToAdd)
    {
        currentScore += scoreToAdd;
        UpdateScoreUI();
    }

    // Method to update the score display in the UI
    void UpdateScoreUI()
    {
        // Set the score text to the current score
        if (scoreText != null)
        {
            scoreText.text = $"Score: {currentScore:D6}"; // Pads score with zeros (e.g., "Score: 000025").

        }
    }

    // Not used yet, maybe for a game over screeen?
    public int GetCurrentScore()
{
    return currentScore;
}

}
