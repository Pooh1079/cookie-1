using UnityEngine;
using UnityEngine.UI;

public class BaseHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public Slider healthSlider;
    public GameObject losePanel;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            DestroyBase();
        }
    }

    void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = (float)currentHealth / maxHealth;
        }
    }

    void DestroyBase()
    {
        if (losePanel != null)
        {
            losePanel.SetActive(true);
            Time.timeScale = 0f;
        }
        Destroy(gameObject);
    }
}


































































