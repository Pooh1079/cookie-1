using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Ссылка на себя

    [Header("Настройки игры")]
    public int zombiesToKill = 10; // Сколько зомби нужно убить для победы
    private int zombiesKilled = 0;  // Счетчик убитых зомби

    [Header("UI элементы")]
    public GameObject winScreen;    // Экран победы
    public GameObject loseScreen;   // Экран поражения

    void Awake()
    {
        // Делаем менеджер доступным из любого скрипта
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Не уничтожаем при загрузке новой сцены
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Вызывается при убийстве зомби
    public void ZombieKilled()
    {
        zombiesKilled++;

        if (zombiesKilled >= zombiesToKill)
        {
            WinGame();
        }
    }

    // Вызывается при разрушении базы
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
        Time.timeScale = 0f; // Останавливаем игру
    }

    void LoseGame()
    {
        if (loseScreen != null)
        {
            Instantiate(loseScreen);
        }
        Time.timeScale = 0f;
    }

    // Для кнопки "В меню"
    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}