

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
    public int maxLevels = 10; // можно изменить в инспекторе

    [Header("Win/Lose")]
    public int zombiesToKill = 10;
    private int zombiesKilled = 0;
    private bool isGameOver = false;

    public GameObject winScreen;
    public GameObject loseScreen;

    [Header("Debug: показывать награду (опционально)")]
    public UnityEngine.UI.Text gemRewardText;

    // 💰 --- Возвращаем деньги для строительства ---
    [Header("Money System (для строительства)")]
    public int money = 100; // стартовое значение
    public UnityEngine.UI.Text moneyText;

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
        if (gemRewardText) gemRewardText.gameObject.SetActive(false);

        UpdateMoneyUI();
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

        // XP (как было)
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
        Debug.Log($"🎉 Победа! +10 XP. Теперь {xp}/{xpToNext}, уровень {lvl}");

        // --- ДОБАВЛЯЕМ 50 МОНЕТ (CoinManager) ---
        if (CoinManager.Instance != null)
        {
            CoinManager.Instance.AddCoins(50);

            if (gemRewardText != null)
            {
                gemRewardText.gameObject.SetActive(true);
                gemRewardText.text = "+50";
            }
        }
        else
        {
            int current = PlayerPrefs.GetInt(CoinManager.CoinsKey, 0);
            current += 50;
            PlayerPrefs.SetInt(CoinManager.CoinsKey, current);
            PlayerPrefs.Save();
            Debug.Log("CoinManager not found — монеты сохранены напрямую в PlayerPrefs.");
        }

        // --- Сохраняем прогресс ---
        int nextLevel = levelNumber + 1;
        if (nextLevel > maxLevels) nextLevel = maxLevels;
        PlayerPrefs.SetInt("currentLevel", nextLevel);
        PlayerPrefs.Save();
        Debug.Log("Next level saved: " + nextLevel);

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

    // 💰 --- Методы для системы строительства ---
    public void AddMoney(int amount)
    {
        money += amount;
        UpdateMoneyUI();
    }

    public bool SpendMoney(int amount)
    {
        if (money >= amount)
        {
            money -= amount;
            UpdateMoneyUI();
            return true;
        }
        return false;
    }

    private void UpdateMoneyUI()
    {
        if (moneyText != null)
            moneyText.text = money.ToString();
    }
}

