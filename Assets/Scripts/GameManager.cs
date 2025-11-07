using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Старт раунда")]
    public bool roundStarted = false;
    public GameObject startButton;

    [Header("Level")]
    public int levelNumber = 1;
    public int maxLevels = 10;

    [Header("Win/Lose")]
    private int zombiesAlive = 0;
    private int zombiesKilled = 0;
    private bool isGameOver = false;
    private bool baseDestroyed = false;
    private bool allZombiesSpawned = false;

    public GameObject winScreen;
    public GameObject loseScreen;

    [Header("Debug: награда (опционально)")]
    public UnityEngine.UI.Text gemRewardText;

    [Header("Money System (для строительства)")]
    public int money = 100;
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
        baseDestroyed = false;
        zombiesKilled = 0;
        zombiesAlive = 0;
        allZombiesSpawned = false;

        // Автоматически определяем номер уровня из имени сцены
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName.StartsWith("Level") && int.TryParse(sceneName.Substring(5), out int levelNum))
        {
            levelNumber = levelNum;
        }

        if (startButton) startButton.SetActive(true);
        if (winScreen) winScreen.SetActive(false);
        if (loseScreen) loseScreen.SetActive(false);
        if (gemRewardText) gemRewardText.gameObject.SetActive(false);

        UpdateMoneyUI();

        // Запускаем проверку завершения спавна
        StartCoroutine(CheckSpawningComplete());
    }

    public void ZombieSpawned()
    {
        zombiesAlive++;
        Debug.Log($"Zombie spawned! Total alive: {zombiesAlive}");
    }

    public void ZombieKilled()
    {
        if (isGameOver || !roundStarted) return;

        zombiesKilled++;
        zombiesAlive--;

        Debug.Log($"Zombies: {zombiesKilled} killed, {zombiesAlive} alive");

        // Проверяем условия победы только если все зомби заспавнены
        if (allZombiesSpawned)
            CheckWinCondition();
    }

    IEnumerator CheckSpawningComplete()
    {
        // Ждем достаточно времени для спавна всех зомби (30 секунд)
        yield return new WaitForSeconds(30f);

        allZombiesSpawned = true;
        Debug.Log("All zombies should be spawned now. Starting win condition checks.");

        // Проверяем сразу на случай если все зомби уже убиты
        CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        // Победа если: все зомби заспавнены И убиты И база не разрушена И раунд начат
        if (allZombiesSpawned && zombiesAlive <= 0 && !baseDestroyed && roundStarted && !isGameOver)
        {
            WinGame();
        }
    }

    void WinGame()
    {
        if (isGameOver) return;
        isGameOver = true;

        // 🟢 Добавляем XP (слава)
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
        Debug.Log($"🏆 Победа! +10 XP. Теперь {xp}/{xpToNext}, уровень {lvl}");

        // 🟡 Добавляем 50 монет через CoinManager
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
            Debug.Log("⚠️ CoinManager не найден — монеты сохранены напрямую в PlayerPrefs.");
        }

        // 🟢 Сохраняем прогресс (следующий уровень)
        int savedLevel = PlayerPrefs.GetInt("currentLevel", 1);
        if (levelNumber >= savedLevel)
        {
            int nextLevel = levelNumber + 1;
            if (nextLevel > maxLevels) nextLevel = maxLevels;
            PlayerPrefs.SetInt("currentLevel", nextLevel);
            PlayerPrefs.Save();
            Debug.Log($"💾 Прогресс сохранен: уровень {nextLevel}");
        }

        if (winScreen != null) winScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    public void BaseDestroyed()
    {
        if (isGameOver || !roundStarted) return;
        baseDestroyed = true;
        isGameOver = true;
        if (loseScreen != null) loseScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    public void StartRound()
    {
        if (roundStarted || isGameOver) return;
        roundStarted = true;
        if (startButton) startButton.SetActive(false);

        // Проверяем условия победы после старта раунда
        if (allZombiesSpawned)
            CheckWinCondition();
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;

        // 💾 Сохраняем монеты и XP перед выходом
        if (CoinManager.Instance != null)
            PlayerPrefs.SetInt(CoinManager.CoinsKey, CoinManager.Instance.Coins);

        if (FameSystem.instance != null)
        {
            PlayerPrefs.SetInt("FameXP", PlayerPrefs.GetInt("FameXP", 0));
            PlayerPrefs.SetInt("FameLevel", PlayerPrefs.GetInt("FameLevel", 1));
        }

        PlayerPrefs.Save();

        SceneManager.LoadScene("MainMenu");
    }

    // 💰 Методы для строительства
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

    // Метод для отладки - показывает текущее состояние
    void Update()
    {
        // Для отладки можно добавить отображение количества зомби
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log($"Debug: Zombies - Killed: {zombiesKilled}, Alive: {zombiesAlive}, AllSpawned: {allZombiesSpawned}");
        }
    }
}