using UnityEngine;
using UnityEngine.UI;

public class BaseHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public Slider healthSlider; // ������ ���� UI Slider �� Canvas (�� �������� ������� ����)
    public GameObject visualsRoot; // (�����������) ������ � �������� ����, ����� ������ ��� ������
    public Collider2D hitCollider; // (�����������) ������ �� ��������� ����

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

        // ��������� ������� � ���������, �� �� ���������� ������� (�� �� Canvas)
        if (visualsRoot != null) visualsRoot.SetActive(false);
        if (hitCollider != null) hitCollider.enabled = false;

        // ��������� ���� ������, ����� ����� ��������� ��� ��������
        enabled = false;

        // �� ������ Destroy(gameObject) ����� � ��� ������ ������, ��� ������ �� UI ��������
    }
}


































































