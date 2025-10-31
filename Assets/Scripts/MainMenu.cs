using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // При старте инициализируем currentLevel, если его нет
    void Start()
    {
        // Устанавливаем default 1, если ключа нет
        if (!PlayerPrefs.HasKey("currentLevel"))
        {
            PlayerPrefs.SetInt("currentLevel", 1);
            PlayerPrefs.Save();
            Debug.Log("MainMenu: currentLevel не найден — установлен в 1");
        }
        else
        {
            Debug.Log("MainMenu: currentLevel = " + PlayerPrefs.GetInt("currentLevel"));
        }

        // Переподключаем CoinManager UI, если он есть
        if (CoinManager.Instance != null)
            CoinManager.Instance.ReconnectUI();

        // Обновляем menu UI (если есть менеджер)
        MenuUIManager menuUI = FindObjectOfType<MenuUIManager>();
        if (menuUI != null) menuUI.RefreshMenuData();
    }

    public void PlayGame()
    {
        int currentLevel = PlayerPrefs.GetInt("currentLevel", 1);

        // защита от кривых значений
        if (currentLevel < 1) currentLevel = 1;
        if (currentLevel > 10) currentLevel = 10;

        string sceneName = "Level" + currentLevel;
        Debug.Log("MainMenu: PlayGame -> loading " + sceneName + " (currentLevel=" + currentLevel + ")");

        SceneManager.LoadScene(sceneName);
    }

    // Кнопка для сброса (можно привязать на кнопку в UI для теста)
    public void ResetProgress()
    {
        PlayerPrefs.SetInt("currentLevel", 1);
        PlayerPrefs.SetInt("PlayerCoins", 0);
        PlayerPrefs.SetInt("FameXP", 0);
        PlayerPrefs.SetInt("FameLevel", 1);
        PlayerPrefs.Save();
        Debug.Log("MainMenu: прогресс сброшен (currentLevel=1, coins=0, fame reset)");
        // обновляем меню UI
        MenuUIManager menuUI = FindObjectOfType<MenuUIManager>();
        if (menuUI != null) menuUI.RefreshMenuData();
        if (CoinManager.Instance != null) CoinManager.Instance.ReconnectUI();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
