using UnityEngine;
using UnityEngine.UI;

public class FameManager : MonoBehaviour
{
    public static FameManager instance;

    [Header("Параметры славы")]
    public int level = 1;
    public int currentXP = 0;
    public int xpToNextLevel = 30;

    [Header("UI")]
    public Slider fameBar;
    public Text levelText;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateUI();
    }

    public void AddXP(int amount)
    {
        currentXP += amount;
        Debug.Log($"Добавлено {amount} XP. Всего: {currentXP}/{xpToNextLevel}");

        if (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }

        UpdateUI();
    }

    void LevelUp()
    {
        currentXP -= xpToNextLevel;
        level++;

        // Настраиваем XP для следующего уровня
        if (level == 2) xpToNextLevel = 40;
        else if (level >= 3) xpToNextLevel = 50;

        Debug.Log($"Новый уровень! {level}. Следующий за {xpToNextLevel} XP");

        UpdateUI();
    }

    void UpdateUI()
    {
        if (fameBar != null)
        {
            fameBar.maxValue = xpToNextLevel;
            fameBar.value = currentXP;
        }

        if (levelText != null)
        {
            levelText.text = $"Уровень: {level}";
        }
    }
}