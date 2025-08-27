using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("����� ������")]
    [Tooltip("���� true � ����� ������� � ����� ������ � ����")]
    public bool roundStarted = false;
    public GameObject startButton; // ���������� ���� ������ Start �� Canvas

    [Header("UI (�������������)")]
    public GameObject winScreen;
    public GameObject loseScreen;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        Time.timeScale = 1f;
        roundStarted = false;
        if (startButton) startButton.SetActive(true);
    }

    // ��������� � OnClick() ������ Start
    public void StartRound()
    {
        if (roundStarted) return;
        Debug.Log("GameManager: StartRound clicked");
        roundStarted = true;
        if (startButton) startButton.SetActive(false);
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    // ��� ������ ���������, ���� ��� ������������ � �������
    public void ZombieKilled() { /* ... */ }
    public void BaseDestroyed()
    {
        if (loseScreen != null) loseScreen.SetActive(true);
        Time.timeScale = 0f;
    }
}