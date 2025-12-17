using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private DisappearingPlatform[] platforms;

    [Header("Tiempo límite")]
    public float timeLimit = 60f;
    private float currentTime;

    [Header("UI")]
    public TextMeshProUGUI timerText;
    public GameObject winPanel;
    public GameObject losePanel;

    private bool gameEnded = false;

    void Start()
    {
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);

        platforms = FindObjectsByType<DisappearingPlatform>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None
        );

        currentTime = timeLimit;
    }

    public void ResetPlatforms()
    {
        foreach (var platform in platforms)
        {
            platform.ResetState();
        }
    }

    void Update()
    {
        if (gameEnded) return;

        currentTime -= Time.deltaTime;

        if (timerText != null)
        {
            timerText.text = "Tiempo: " + Mathf.Ceil(currentTime).ToString();
        }

        if (currentTime <= 0)
        {
            LoseGame();
        }
    }

    public void WinGame()
    {
        gameEnded = true;
        if (winPanel != null) winPanel.SetActive(true);
        Debug.Log("¡Has ganado!");
    }

    public void LoseGame()
    {
        gameEnded = true;
        if (losePanel != null) losePanel.SetActive(true);
        Debug.Log("Has perdido...");
    }
}