using UnityEngine;
using UnityEngine.UI;
using TMPro; // если используешь TextMeshPro

public class MoneyUI : MonoBehaviour
{
    public TextMeshProUGUI moneyText; // если TMP
    // public Text moneyText; // если обычный Text

    private int lastMoney = -1;

    void Update()
    {
        if (GameManager.instance == null)
            return;

        var moneyField = GameManager.instance.GetType().GetField("money");
        if (moneyField != null)
        {
            int currentMoney = (int)moneyField.GetValue(GameManager.instance);

            // обновляем текст только если значение изменилось
            if (currentMoney != lastMoney)
            {
                lastMoney = currentMoney;
                moneyText.text = $"💵 Деньги: {currentMoney}";
            }
        }
    }
}
