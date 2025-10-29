using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;
    public const string CoinsKey = "PlayerCoins";

    [Header("UI для отображения монет:")]
    [Tooltip("Если используешь TextMeshPro — перетащи сюда TMP Text. Если обычный UI Text — перетащи в 'coinsUIText'.")]
    public TMP_Text coinsTMP;   // поле для TextMeshPro (опционально)
    public Text coinsUIText;    // поле для старого UnityEngine.UI.Text (опционально)

    // событие для подписки (MainMenu, другие UI)
    public event Action<int> OnCoinsChanged;

    private int coins;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        coins = PlayerPrefs.GetInt(CoinsKey, 0);
        UpdateUI();
    }

    public int GetCoins()
    {
        return coins;
    }

    public void AddCoins(int amount)
    {
        if (amount == 0) return;
        coins += amount;
        PlayerPrefs.SetInt(CoinsKey, coins);
        PlayerPrefs.Save();

        // оповещаем подписчиков
        OnCoinsChanged?.Invoke(coins);
        UpdateUI();
        Debug.Log($"CoinManager: +{amount} монет (всего {coins})");
    }

    public bool TrySpendCoins(int amount)
    {
        if (amount <= 0) return true;
        if (coins < amount) return false;
        coins -= amount;
        PlayerPrefs.SetInt(CoinsKey, coins);
        PlayerPrefs.Save();
        OnCoinsChanged?.Invoke(coins);
        UpdateUI();
        return true;
    }

    public void SpendCoins(int amount)
    {
        // безопасное списание без возврата
        coins = Mathf.Max(0, coins - amount);
        PlayerPrefs.SetInt(CoinsKey, coins);
        PlayerPrefs.Save();
        OnCoinsChanged?.Invoke(coins);
        UpdateUI();
    }

    void UpdateUI()
    {
        // если задан TMP — обновляем его
        if (coinsTMP != null)
            coinsTMP.text = coins.ToString();

        // иначе, если задан обычный Text — обновляем его
        if (coinsUIText != null)
            coinsUIText.text = coins.ToString();
    }
}
