using UnityEngine;
using UnityEngine.UI;

public class FameSystem : MonoBehaviour
{
    public static FameSystem instance;

    [Header("UI элементы")]
    public Slider xpBar;
    public Text xpText;
    public Text levelText;

    private int currentXP;
    private int currentLevel;
    private int xpToNextLevel;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Загружаем сохранённые данные
        currentXP = PlayerPrefs.GetInt("FameXP", 0);
        currentLevel = PlayerPrefs.GetInt("FameLevel", 1);
        UpdateXPToNextLevel();
        UpdateUI();
    }

    void UpdateXPToNextLevel()
    {
        if (currentLevel == 1)
            xpToNextLevel = 30;
        else if (currentLevel == 2)
            xpToNextLevel = 40;
        else
            xpToNextLevel = 50;
    }

    public void AddXP(int amount)
    {
        currentXP += amount;
        Debug.Log($"🎖 Добавлено {amount} XP (всего {currentXP}/{xpToNextLevel})");

        // Проверяем, достаточно ли для повышения уровня
        while (currentXP >= xpToNextLevel)
        {
            currentXP -= xpToNextLevel;
            currentLevel++;
            UpdateXPToNextLevel();
            Debug.Log($"🎉 Уровень повышен! Теперь {currentLevel}");
        }

        SaveData();
        UpdateUI();
    }

    void SaveData()
    {
        PlayerPrefs.SetInt("FameXP", currentXP);
        PlayerPrefs.SetInt("FameLevel", currentLevel);
        PlayerPrefs.Save();
    }

    void UpdateUI()
    {
        if (xpBar != null)
        {
            xpBar.maxValue = xpToNextLevel;
            xpBar.value = currentXP;
        }

        if (xpText != null)
            xpText.text = $"{currentXP}/{xpToNextLevel} XP";

        if (levelText != null)
            levelText.text = $"Уровень: {currentLevel}";
    }
}

