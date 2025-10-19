using UnityEngine;
using UnityEngine.UI;

public class FameManager : MonoBehaviour
{
    public static FameManager instance;

    [Header("��������� �����")]
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
        Debug.Log($"��������� {amount} XP. �����: {currentXP}/{xpToNextLevel}");

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

        // ����������� XP ��� ���������� ������
        if (level == 2) xpToNextLevel = 40;
        else if (level >= 3) xpToNextLevel = 50;

        Debug.Log($"����� �������! {level}. ��������� �� {xpToNextLevel} XP");

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
            levelText.text = $"�������: {level}";
        }
    }
}