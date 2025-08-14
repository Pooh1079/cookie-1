using UnityEngine;
using UnityEngine.UI;

public class BaseHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public Slider healthSlider; // Должен быть UI Slider на Canvas (НЕ дочерний элемент базы)
    public GameObject visualsRoot; // (опционально) объект с визуалом базы, чтобы скрыть при смерти
    public Collider2D hitCollider; // (опционально) ссылка на коллайдер базы

    void Start()
    {
        currentHealth = maxHealth;
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log($"BaseHealth.TakeDamage({amount}) => {currentHealth}");
        if (healthSlider != null)
            healthSlider.value = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        Debug.Log("BaseHealth.Die() called");
        if (GameManager.instance != null)
            GameManager.instance.BaseDestroyed();

        // Отключаем визуалы и коллайдер, но НЕ уничтожаем слайдер (он на Canvas)
        if (visualsRoot != null) visualsRoot.SetActive(false);
        if (hitCollider != null) hitCollider.enabled = false;

        // Отключаем этот скрипт, чтобы зомби перестали его вызывать
        enabled = false;

        // Не делаем Destroy(gameObject) здесь — так меньше рисков, что ссылки на UI исчезнут
    }
}


































































