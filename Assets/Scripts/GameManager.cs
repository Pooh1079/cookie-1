using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Старт раунда")]
    public bool roundStarted = false;
    public GameObject startButton;

    [Header("Level")]
    public int levelNumber = 1;
    public int maxLevels = 3;

    [Header("Win/Lose")]
    public int zombiesToKill = 10;
    private int zombiesKilled = 0;
    private bool isGameOver = false;

    public GameObject winScreen;
    public GameObject loseScreen;

    [Header("Деньги игрока")]
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

    public void StartRound()
    {
        if (roundStarted || isGameOver) return;
        roundStarted = true;
        if (startButton) startButton.SetActive(false);
    }

    public void ZombieKilled()
    {
        if (isGameOver || !roundStarted) return;
        zombiesKilled++;
        if (zombiesKilled >= zombiesToKill)
            WinGame();
    }

    void WinGame()
    {
        if (isGameOver) return;
        isGameOver = true;

        // 🎖 Добавляем XP за победу через PlayerPrefs
        int xp = PlayerPrefs.GetInt("FameXP", 0);
        int lvl = PlayerPrefs.GetInt("FameLevel", 1);

        int xpToNext = lvl == 1 ? 30 : (lvl == 2 ? 40 : 50);

        xp += 10;
        if (xp >= xpToNext)
        {
            xp -= xpToNext;
            lvl++;
        }

        PlayerPrefs.SetInt("FameXP", xp);
        PlayerPrefs.SetInt("FameLevel", lvl);
        PlayerPrefs.Save();
        Debug.Log($"🎉 Победа! Добавлено 10 XP. Теперь {xp}/{xpToNext}, уровень {lvl}");

        int nextLevel = Mathf.Clamp(levelNumber + 1, 1, maxLevels);
        PlayerPrefs.SetInt("currentLevel", nextLevel);
        PlayerPrefs.Save();

        if (winScreen != null) winScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    public void BaseDestroyed()
    {
        if (isGameOver || !roundStarted) return;
        isGameOver = true;
        if (loseScreen != null) loseScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}

