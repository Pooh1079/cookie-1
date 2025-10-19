using UnityEngine;
using UnityEngine.UI;

public class BaseHealth : MonoBehaviour
{
    [Header("��������� ��������")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("UI")]
    public Text baseHealthText; // ���� �������� ������ UI Text

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthText();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        Debug.Log("���� �������� ����! ������� HP: " + currentHealth);

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
            Debug.LogWarning("BaseHealthText �� �������� � ����������!");
            return;
        }

        // ��������� ��� �����
        baseHealthText.text = "HP: " + currentHealth;

        // ���������� ������� ����������� �������� (0�1)
        float healthPercent = (float)currentHealth / maxHealth;

        // ������ ��� 100%, ����� ��� 50%, ������� ��� 0%
        Color healthColor;
        if (healthPercent > 0.5f)
        {
            // �� ������� � ������
            float t = (healthPercent - 0.5f) / 0.5f;
            healthColor = Color.Lerp(Color.yellow, Color.green, t);
        }
        else
        {
            // �� ������ � ��������
            float t = healthPercent / 0.5f;
            healthColor = Color.Lerp(Color.red, Color.yellow, t);
        }

        // ��������� ���� � ������
        baseHealthText.color = healthColor;
    }
}