using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;

    [System.Serializable]
    public class TurretBlueprint
    {
        public string name;
        public GameObject prefab;
        public int cost;
        public Button buildButton;
    }

    [Header("Список доступных турелей")]
    public TurretBlueprint[] turrets;

    [Header("Ссылки на UI")]
    public GameObject buildMenuPanel;
    public Button cancelButton;
    public Button buildModeButton;
    public Text moneyText; // 💰 Отображение денег
    public Text warningText; // ⚠️ Сообщение об ошибке ("Недостаточно денег")

    [Header("Debug - Текущая турель")]
    public TurretBlueprint selectedTurret;

    [Header("Режим строительства")]
    public bool isBuilding = false; // Флаг: активен ли режим строительства

    private int lastMoney = -1;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Больше одного BuildManager на сцене!");
            return;
        }
        instance = this;
    }

    void Start()
    {
        InitializeBuildButtons();

        buildMenuPanel.SetActive(false);
        cancelButton.gameObject.SetActive(false);
        isBuilding = false;

        // 💰 Прячем предупреждение при старте
        if (warningText != null)
            warningText.gameObject.SetActive(false);

        UpdateMoneyUI(true);
    }

    void Update()
    {
        // 💰 обновляем деньги только если изменились
        UpdateMoneyUI(false);
    }

    // =====================================================
    // ===  UI: Деньги ====================================
    // =====================================================
    void UpdateMoneyUI(bool force)
    {
        if (GameManager.instance == null || moneyText == null)
            return;

        int currentMoney = GameManager.instance.money;
        if (currentMoney != lastMoney || force)
        {
            lastMoney = currentMoney;
            moneyText.text = $"💵 Деньги: {currentMoney}";
        }
    }

    IEnumerator ShowWarning(string text)
    {
        if (warningText == null) yield break;

        warningText.text = text;
        warningText.color = new Color(1, 0.2f, 0.2f, 1f); // красный цвет
        warningText.gameObject.SetActive(true);

        // Плавное исчезновение
        float duration = 2f;
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            Color c = warningText.color;
            c.a = Mathf.Lerp(1f, 0f, timer / duration);
            warningText.color = c;
            yield return null;
        }

        warningText.gameObject.SetActive(false);
    }

    // =====================================================
    // ===  КНОПКИ СТРОИТЕЛЬСТВА ==========================
    // =====================================================
    void InitializeBuildButtons()
    {
        for (int i = 0; i < turrets.Length; i++)
        {
            int index = i;
            if (turrets[i].buildButton != null)
            {
                turrets[i].buildButton.onClick.RemoveAllListeners();
                turrets[i].buildButton.onClick.AddListener(() => SelectTurret(index));
            }
            else
            {
                Debug.LogWarning($"У турели {turrets[i].name} нет кнопки buildButton!");
            }
        }
    }

    public void ToggleBuildMenu()
    {
        selectedTurret = null;
        isBuilding = false;
        bool isMenuActive = buildMenuPanel.activeSelf;
        buildMenuPanel.SetActive(!isMenuActive);
        cancelButton.gameObject.SetActive(false);
    }

    public void SelectTurret(int turretIndex)
    {
        if (turretIndex < 0 || turretIndex >= turrets.Length)
        {
            Debug.LogError("Неверный индекс турели!");
            return;
        }

        selectedTurret = turrets[turretIndex];

        if (selectedTurret.prefab == null)
        {
            Debug.LogError($"У турели '{selectedTurret.name}' отсутствует префаб!");
            selectedTurret = null;
            return;
        }

        buildMenuPanel.SetActive(false);
        cancelButton.gameObject.SetActive(true);

        isBuilding = true;
        Debug.Log($"Выбрана турель: {selectedTurret.name}, режим строительства включён");
    }

    public void CancelTurretSelection()
    {
        selectedTurret = null;
        isBuilding = false;
        cancelButton.gameObject.SetActive(false);
        Debug.Log("Режим строительства отменён");
    }

    // =====================================================
    // ===  ПОСТРОЙКА ТУРЕЛИ ==============================
    // =====================================================
    public void BuildTurretOn(Vector3 buildPosition)
    {
        if (selectedTurret == null || selectedTurret.prefab == null)
        {
            Debug.LogError("Не выбрана турель или отсутствует префаб!");
            return;
        }

        if (GameManager.instance != null && GameManager.instance.money < selectedTurret.cost)
        {
            Debug.Log("Недостаточно денег для строительства!");
            StartCoroutine(ShowWarning("Недостаточно денег!"));
            CancelTurretSelection();
            return;
        }

        GameObject newTurret = Instantiate(selectedTurret.prefab, buildPosition, Quaternion.identity);
        Debug.Log($"Турель '{selectedTurret.name}' построена!");

        if (GameManager.instance != null)
        {
            GameManager.instance.money -= selectedTurret.cost;
            UpdateMoneyUI(true);
            Debug.Log($"Списано {selectedTurret.cost} денег. Осталось: {GameManager.instance.money}");
        }

        CancelTurretSelection();
    }
}

