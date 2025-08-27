using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Старт раунда")]
    [Tooltip("Если true — зомби активны и будут ходить к базе")]
    public bool roundStarted = false;
    public GameObject startButton; // перетащить сюда кнопку Start из Canvas

    [Header("UI (необязательно)")]
    public GameObject winScreen;
    public GameObject loseScreen;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        Time.timeScale = 1f;
        roundStarted = false;
        if (startButton) startButton.SetActive(true);
    }

    // Привязать к OnClick() кнопки Start
    public void StartRound()
    {
        if (roundStarted) return;
        Debug.Log("GameManager: StartRound clicked");
        roundStarted = true;
        if (startButton) startButton.SetActive(false);
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    // Эти методы оставляем, если уже используются в проекте
    public void ZombieKilled() { /* ... */ }
    public void BaseDestroyed()
    {
        if (loseScreen != null) loseScreen.SetActive(true);
        Time.timeScale = 0f;
    }
}