using UnityEngine;
using UnityEngine.UI;

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

    // Сделаем public для отладки
    [Header("Debug - Текущая турель")]
    public TurretBlueprint selectedTurret;

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
        Debug.Log("=== START ===");
        Debug.Log("turrets.Length: " + turrets.Length);
        Debug.Log("selectedTurret: " + (selectedTurret == null ? "null" : selectedTurret.name));

        // ИНИЦИАЛИЗАЦИЯ КНОПОК - ВАЖНО!
        InitializeBuildButtons();

        buildMenuPanel.SetActive(false);
        cancelButton.gameObject.SetActive(false);
    }

    // НОВЫЙ МЕТОД: Инициализация кнопок строительства
    void InitializeBuildButtons()
    {
        for (int i = 0; i < turrets.Length; i++)
        {
            int index = i; // Важно: создаем локальную копию для замыкания
            if (turrets[i].buildButton != null)
            {
                // Убираем старые обработчики и добавляем новый
                turrets[i].buildButton.onClick.RemoveAllListeners();
                turrets[i].buildButton.onClick.AddListener(() => SelectTurret(index));
                Debug.Log($"Кнопка инициализирована для турели: {turrets[i].name}, индекс: {index}");
            }
            else
            {
                Debug.LogWarning($"У турели {turrets[i].name} нет кнопки buildButton!");
            }
        }
    }

    void Update()
    {
        // Отладочная информация каждый кадр (временно)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("=== DEBUG INFO ===");
            Debug.Log("selectedTurret: " + (selectedTurret == null ? "null" : selectedTurret.name));
            Debug.Log("turrets count: " + turrets.Length);
            Debug.Log("buildMenuPanel active: " + buildMenuPanel.activeSelf);
        }
    }

    public void ToggleBuildMenu()
    {
        Debug.Log("=== TOGGLE BUILD MENU ===");
        Debug.Log("До открытия - selectedTurret: " + (selectedTurret == null ? "null" : selectedTurret.name));

        // СБРАСЫВАЕМ ВСЕГДА при открытии/закрытии меню
        selectedTurret = null;

        bool isMenuActive = buildMenuPanel.activeSelf;
        buildMenuPanel.SetActive(!isMenuActive);

        // Скрываем кнопку отмены при открытии/закрытии меню
        cancelButton.gameObject.SetActive(false);

        // ПРОВЕРКА: убедимся что selectedTurret остался null
        if (selectedTurret != null)
        {
            Debug.LogError("ОШИБКА: selectedTurret стал не-null после сброса! Это: " + selectedTurret.name);
            selectedTurret = null; // Принудительно сбрасываем
        }

        Debug.Log("После открытия - selectedTurret: " + (selectedTurret == null ? "null" : "NOT NULL"));
        Debug.Log("Меню теперь: " + (!isMenuActive ? "открыто" : "закрыто"));
    }

    public void SelectTurret(int turretIndex)
    {
        Debug.Log("=== SELECT TURRET ===");
        Debug.Log("Индекс: " + turretIndex);
        Debug.Log("Размер массива: " + turrets.Length);

        if (turretIndex < 0 || turretIndex >= turrets.Length)
        {
            Debug.LogError("Неверный индекс турели!");
            return;
        }

        selectedTurret = turrets[turretIndex];

        // ДОПОЛНИТЕЛЬНАЯ ПРОВЕРКА ПРЕФАБА
        if (selectedTurret.prefab == null)
        {
            Debug.LogError($"У турели '{selectedTurret.name}' отсутствует префаб!");
            selectedTurret = null;
            return;
        }

        Debug.Log("Выбрана турель: " + selectedTurret.name + " (префаб: " + (selectedTurret.prefab != null ? "есть" : "отсутствует") + ")");

        buildMenuPanel.SetActive(false);
        cancelButton.gameObject.SetActive(true);
    }

    public void CancelTurretSelection()
    {
        Debug.Log("=== CANCEL SELECTION ===");
        Debug.Log("До отмены - selectedTurret: " + (selectedTurret == null ? "null" : selectedTurret.name));

        selectedTurret = null;
        cancelButton.gameObject.SetActive(false);

        Debug.Log("После отмены - selectedTurret: " + (selectedTurret == null ? "null" : "NOT NULL"));
    }

    public void BuildTurretOn(Vector3 buildPosition)
    {
        Debug.Log("=== BUILD TURRET ===");
        Debug.Log("selectedTurret: " + (selectedTurret == null ? "null" : selectedTurret.name));

        if (selectedTurret == null)
        {
            Debug.LogError("selectedTurret is NULL!");
            return;
        }

        if (selectedTurret.prefab == null)
        {
            Debug.LogError("Prefab is NULL for turret: " + (selectedTurret.name ?? "NO NAME"));
            return;
        }

        // ПРОВЕРКА ДЕНЕГ
        if (GameManager.instance != null && GameManager.instance.money < selectedTurret.cost)
        {
            Debug.Log("Недостаточно денег для строительства!");
            CancelTurretSelection();
            return;
        }

        GameObject newTurret = Instantiate(selectedTurret.prefab, buildPosition, Quaternion.identity);
        Debug.Log("Турель построена!");

        // СПИСАНИЕ ДЕНЕГ
        if (GameManager.instance != null)
        {
            GameManager.instance.money -= selectedTurret.cost;
            Debug.Log($"Списано {selectedTurret.cost} денег. Осталось: {GameManager.instance.money}");
        }

        CancelTurretSelection();
    }
}