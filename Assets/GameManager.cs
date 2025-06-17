using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // ������ �� ����

    [Header("��������� ����")]
    public int zombiesToKill = 10; // ������� ����� ����� ����� ��� ������
    private int zombiesKilled = 0;  // ������� ������ �����

    [Header("UI ��������")]
    public GameObject winScreen;    // ����� ������
    public GameObject loseScreen;   // ����� ���������

    void Awake()
    {
        // ������ �������� ��������� �� ������ �������
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // �� ���������� ��� �������� ����� �����
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ���������� ��� �������� �����
    public void ZombieKilled()
    {
        zombiesKilled++;

        if (zombiesKilled >= zombiesToKill)
        {
            WinGame();
        }
    }

    // ���������� ��� ���������� ����
    public void BaseDestroyed()
    {
        LoseGame();
    }

    void WinGame()
    {
        if (winScreen != null)
        {
            Instantiate(winScreen);
        }
        Time.timeScale = 0f; // ������������� ����
    }

    void LoseGame()
    {
        if (loseScreen != null)
        {
            Instantiate(loseScreen);
        }
        Time.timeScale = 0f;
    }

    // ��� ������ "� ����"
    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}