using UnityEngine;

public class ZombieHealth : MonoBehaviour
{
    [Header("��������� ��������")]
    public int maxHealth = 20;  // ������������ ��������
    private int currentHealth;   // ������� ��������

    [Header("���������� �������")]
    public GameObject damageEffect;  // ������ ��� ��������� �����
    public GameObject deathEffect;   // ������ ��� ������

    void Start()
    {
        currentHealth = maxHealth;  // ������������� ������ �������� ��� ������
    }

    // ����� ��� ��������� �����
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;  // ��������� ��������

        // ������� ������ ���������
        if (damageEffect != null)
        {
            Instantiate(damageEffect, transform.position, Quaternion.identity);
        }

        // ��������� ������
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // ����� ������
    void Die()
    {
        // ������ ������
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        // �������� GameManager � ������
        if (GameManager.instance != null)
        {
            GameManager.instance.ZombieKilled();
        }

        Destroy(gameObject);  // ���������� �����

        // ������ ������
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        // ���������� GameManager (� ��������� �� null)
        if (GameManager.instance != null)
        {
            GameManager.instance.ZombieKilled();
        }
        else
        {
            Debug.LogWarning("GameManager �� ������!");
        }

        Destroy(gameObject);
    }
}   