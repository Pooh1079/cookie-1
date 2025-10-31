using UnityEngine;
using UnityEngine.UI;

public class MenuUIManager : MonoBehaviour
{
    [Header("UI Элементы")]
    public Text coinsText;
    public Text fameText;
    public Image fameBar;
    public Image[] uiImages; // если хочешь, чтобы картинки не исчезали

    void Start()
    {
        // Подписываемся на изменение монет
        if (CoinManager.Instance != null)
            CoinManager.Instance.OnCoinsChanged += UpdateCoinsUI;

        Invoke(nameof(UpdateAllUI), 0.1f);
    }

    public void UpdateAllUI()
    {
        UpdateCoinsUI(CoinManager.Instance != null ? CoinManager.Instance.Coins : PlayerPrefs.GetInt(CoinManager.CoinsKey, 0));
        UpdateFameUI();
    }

    public void UpdateCoinsUI(int value)
    {
        if (coinsText != null)
            coinsText.text = value.ToString();
    }

    public void UpdateFameUI()
    {
        int xp = PlayerPrefs.GetInt("FameXP", 0);
        int lvl = PlayerPrefs.GetInt("FameLevel", 1);
        int xpToNext = lvl == 1 ? 30 : (lvl == 2 ? 40 : 50);

        if (fameText != null)
            fameText.text = $"Lv {lvl}  {xp}/{xpToNext}";

        if (fameBar != null)
            fameBar.fillAmount = (float)xp / xpToNext;
    }

    public void RefreshMenuData()
    {
        UpdateAllUI();
    }
}
