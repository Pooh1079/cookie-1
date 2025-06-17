using UnityEngine;

public class ZombieHealth : MonoBehaviour
{
    [Header("Настройки здоровья")]
    public int maxHealth = 20;  // Максимальное здоровье
    private int currentHealth;   // Текущее здоровье

    [Header("Визуальные эффекты")]
    public GameObject damageEffect;  // Эффект при получении урона
    public GameObject deathEffect;   // Эффект при смерти

    void Start()
    {
        currentHealth = maxHealth;  // Устанавливаем полное здоровье при старте
    }

    // Метод для получения урона
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;  // Уменьшаем здоровье

        // Создаем эффект попадания
        if (damageEffect != null)
        {
            Instantiate(damageEffect, transform.position, Quaternion.identity);
        }

        // Проверяем смерть
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Метод смерти
    void Die()
    {
        // Эффект смерти
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        // Сообщаем GameManager о смерти
        if (GameManager.instance != null)
        {
            GameManager.instance.ZombieKilled();
        }

        Destroy(gameObject);  // Уничтожаем зомби

        // Эффект смерти
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        // Уведомляем GameManager (с проверкой на null)
        if (GameManager.instance != null)
        {
            GameManager.instance.ZombieKilled();
        }
        else
        {
            Debug.LogWarning("GameManager не найден!");
        }

        Destroy(gameObject);
    }
}   