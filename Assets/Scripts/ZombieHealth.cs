using UnityEngine;

public class ZombieHealth : MonoBehaviour
{
    public int maxHealth = 20;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        if (GameManager.instance != null)
            GameManager.instance.ZombieKilled();
        Destroy(gameObject);
    }
}