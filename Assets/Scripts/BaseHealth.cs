using UnityEngine;
using UnityEngine.UI;

public class BaseHealth : MonoBehaviour
{
    [Header("Настройки здоровья")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("UI")]
    public Text baseHealthText; // Сюда перетащи объект UI Text

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthText();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        Debug.Log("База получила урон! Текущее HP: " + currentHealth);

        UpdateHealthText();

        if (currentHealth <= 0)
        {
            if (GameManager.instance != null)
                GameManager.instance.BaseDestroyed();
        }
    }

    void UpdateHealthText()
    {
        if (baseHealthText == null)
        {
            Debug.LogWarning("BaseHealthText не назначен в инспекторе!");
            return;
        }

        // Обновляем сам текст
        baseHealthText.text = "HP: " + currentHealth;

        // Определяем процент оставшегося здоровья (0–1)
        float healthPercent = (float)currentHealth / maxHealth;

        // Зелёный при 100%, жёлтый при 50%, красный при 0%
        Color healthColor;
        if (healthPercent > 0.5f)
        {
            // от зелёного к жёлтому
            float t = (healthPercent - 0.5f) / 0.5f;
            healthColor = Color.Lerp(Color.yellow, Color.green, t);
        }
        else
        {
            // от жёлтого к красному
            float t = healthPercent / 0.5f;
            healthColor = Color.Lerp(Color.red, Color.yellow, t);
        }

        // Применяем цвет к тексту
        baseHealthText.color = healthColor;
    }
}