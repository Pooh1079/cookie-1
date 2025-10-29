using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [Header("UI для отображения монет в меню (опционально):")]
    public TMP_Text coinsTMP; // если используешь TextMeshPro в меню
    public Text coinsUIText;  // если используешь обычный UI Text в меню

    void Start()
    {
        // Подписываемся на событие изменения монет, если CoinManager есть
        if (CoinManager.Instance != null)
        {
            CoinManager.Instance.OnCoinsChanged += UpdateCoinsUI;
            // сразу обновим
            UpdateCoinsUI(CoinManager.Instance.GetCoins());
        }
        else
        {
            // Если CoinManager ещё не создан, прочитаем из PlayerPrefs и покажем
            int saved = PlayerPrefs.GetInt(CoinManager.CoinsKey, 0);
            UpdateCoinsUI(saved);
        }
    }

    void OnDestroy()
    {
        if (CoinManager.Instance != null)
            CoinManager.Instance.OnCoinsChanged -= UpdateCoinsUI;
    }

    void UpdateCoinsUI(int amount)
    {
        if (coinsTMP != null) coinsTMP.text = amount.ToString();
        if (coinsUIText != null) coinsUIText.text = amount.ToString();
    }

    public void PlayGame()
    {
        int currentLevel = PlayerPrefs.GetInt("currentLevel", 1);
        if (currentLevel < 1) currentLevel = 1;
        if (currentLevel > 10) currentLevel = 10;

        string levelName = "Level" + currentLevel;
        Debug.Log("MainMenu: loading " + levelName);
        SceneManager.LoadScene(levelName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

