using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject winScreen;
    public GameObject loseScreen;
    public int zombiesToKill = 10;

    private int zombiesKilled = 0;
    public bool isGameOver = false;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void ZombieKilled()
    {
        zombiesKilled++;
        Debug.Log($"GameManager: ZombieKilled {zombiesKilled}/{zombiesToKill}");
        if (zombiesKilled >= zombiesToKill) WinGame();
    }

    public void BaseDestroyed()
    {
        if (isGameOver) return;
        isGameOver = true;
        Debug.Log("GameManager: BaseDestroyed called");
        if (loseScreen != null) loseScreen.SetActive(true);
        else Debug.LogWarning("GameManager: loseScreen NOT assigned in Inspector!");
        // show UI first, then stop time
        Time.timeScale = 0f;
    }

    void WinGame()
    {
        if (isGameOver) return;
        isGameOver = true;
        if (winScreen != null) winScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}