using UnityEngine;
using UnityEngine.UI;
using System;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;
    public const string CoinsKey = "PlayerCoins";

    public int Coins { get; private set; }

    [Header("UI Ссылка на текст монет")]
    public Text coinsText;

    public event Action<int> OnCoinsChanged;

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

        Coins = PlayerPrefs.GetInt(CoinsKey, 0);
        UpdateCoinsUI();
    }

    public void AddCoins(int amount)
    {
        Coins += amount;
        PlayerPrefs.SetInt(CoinsKey, Coins);
        PlayerPrefs.Save();
        UpdateCoinsUI();
        OnCoinsChanged?.Invoke(Coins);
    }

    public void SpendCoins(int amount)
    {
        Coins = Mathf.Max(0, Coins - amount);
        PlayerPrefs.SetInt(CoinsKey, Coins);
        PlayerPrefs.Save();
        UpdateCoinsUI();
        OnCoinsChanged?.Invoke(Coins);
    }

    public void UpdateCoinsUI()
    {
        if (coinsText != null)
            coinsText.text = Coins.ToString();
    }

    // 🔁 Вызывается из MainMenu при возврате
    public void ReconnectUI()
    {
        UpdateCoinsUI();
        OnCoinsChanged?.Invoke(Coins);
    }
}
