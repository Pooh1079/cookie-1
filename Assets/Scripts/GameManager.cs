using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Старт раунда")]
    [Tooltip("Если true — зомби активны и будут ходить к базе")]
    public bool roundStarted = false;
    public GameObject startButton; // перетащить сюда кнопку Start из Canvas

    [Header("Level")]
    public int levelNumber = 1;     // выставь в Inspector: 1 для Level1, 2 для Level2, 3 для Level3
    public int maxLevels = 3;

    [Header("Win/Lose")]
    public int zombiesToKill = 10;
    private int zombiesKilled = 0;
    private bool isGameOver = false;

    public GameObject winScreen; // по желанию
    public GameObject loseScreen;

    public int money = 200;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        Time.timeScale = 1f;
        roundStarted = false;
        isGameOver = false;
        zombiesKilled = 0;

        if (startButton) startButton.SetActive(true);
        if (winScreen) winScreen.SetActive(false);
        if (loseScreen) loseScreen.SetActive(false);
    }

    // Привязать к OnClick() кнопки Start
    public void StartRound()
    {
        if (roundStarted || isGameOver) return;
        Debug.Log("GameManager: StartRound clicked");
        roundStarted = true;
        if (startButton) startButton.SetActive(false);
    }

    public void ZombieKilled()
    {
        if (isGameOver || !roundStarted) return;
        zombiesKilled++;
        Debug.Log($"GameManager: ZombieKilled {zombiesKilled}/{zombiesToKill}");
        if (zombiesKilled >= zombiesToKill) WinGame();
    }

    void WinGame()
    {
        if (isGameOver) return;
        isGameOver = true;
        Debug.Log("GameManager: WinGame for level " + levelNumber);

        if (FameManager.instance != null)
            FameManager.instance.AddXP(10); // +10 XP за победу

        int nextLevel = Mathf.Clamp(levelNumber + 1, 1, maxLevels);
        PlayerPrefs.SetInt("currentLevel", nextLevel);
        PlayerPrefs.Save();

        if (winScreen != null) winScreen.SetActive(true);
        Time.timeScale = 0f;
        Debug.Log("XP добавлен!");
    }

    public void BaseDestroyed()
    {
        if (isGameOver || !roundStarted) return;
        isGameOver = true;
        Debug.Log("GameManager: BaseDestroyed");
        if (loseScreen != null) loseScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    // кнопка "В меню" в Win/Lose (если есть)
    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}