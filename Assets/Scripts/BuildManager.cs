using UnityEngine;
using UnityEngine.UI; // Важно для работы с UI элементами

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;

    [Header("Префабы турелей")]
    public GameObject crossbowPrefab; // Перетащите сюда префаб Prefab_Crossbow

    [Header("Стоимость турелей")]
    public int crossbowCost = 100;

    [Header("Ссылки на UI")]
    public GameObject buildMenuPanel; // Панель с выбором турели
    public Button buildCrossbowButton; // Конкретная кнопка "Построить Арбалет"
    public Button cancelButton; // Кнопка отмены
    public Button buildModeButton; // Главная кнопка "Build"

    // Какая турель выбрана для постройки?
    public GameObject turretToBuild;
    // Достаточно ли денег?
    private bool hasEnoughMoney = false;

    void Awake()
    {
        // Делаем менеджер одиночкой (Singleton)
        if (instance != null)
        {
            Debug.LogError("Больше одного BuildManager на сцене!");
            return;
        }
        instance = this;
    }

    void Start()
    {
        // Обновляем интерфейс при старте
        UpdateUI();

        // Сразу скрываем меню выбора турелей
        if (buildMenuPanel != null)
            buildMenuPanel.SetActive(false);

        // Делаем кнопку отмены неактивной
        if (cancelButton != null)
            cancelButton.gameObject.SetActive(false);
    }

    void Update()
    {
        // Постоянно проверяем, хватает ли денег на выбранную турель
        // (Здесь предполагается, что у GameManager есть переменная 'money')
         hasEnoughMoney = (GameManager.instance.money >= crossbowCost);

        // Если турель выбрана и мы в режиме строительства, можно добавить визуальный индикатор
        // (например, полупрозрачный призрак турели, следующий за курсором)
        if (turretToBuild != null)
        {
            // Логика отображения "призрака" турели
        }

        // Обработка отмены строительства по правой кнопке мыши или ESC
        if (turretToBuild != null && (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape)))
        {
            CancelTurretSelection();
        }
    }

    // Вызывается по клику на главную кнопку "Build"
    public void ToggleBuildMenu()
    {
        // Включаем или выключаем меню выбора турелей
        bool isMenuActive = buildMenuPanel.activeSelf;
        buildMenuPanel.SetActive(!isMenuActive);

        // Если мы открыли меню, выходим из режима строительства
        if (!isMenuActive && turretToBuild != null)
        {
            CancelTurretSelection();
        }
    }

    // Вызывается по клику на кнопку "Построить Арбалет"
    public void SelectTurretToBuild()
    {
        // 1. Проверить деньги (if (!hasEnoughMoney) return;)
        // 2. Выбрать турель
        turretToBuild = crossbowPrefab;
        Debug.Log("Выбрана турель для постройки: " + turretToBuild.name);

        // 3. Закрыть меню выбора
        buildMenuPanel.SetActive(false);

        // 4. Активировать кнопку отмены
        cancelButton.gameObject.SetActive(true);

        // 5. Здесь можно включить визуальный индикатор (призрак турели)
    }

    // Вызывается по клику на кнопку "Cancel" или правой кнопкой мыши
    public void CancelTurretSelection()
    {
        turretToBuild = null;
        Debug.Log("Режим строительства отменен.");

        // Деактивируем кнопку отмены
        cancelButton.gameObject.SetActive(false);

        // Скрываем визуальный индикатор
    }

    // Этот метод будет вызываться из другого скрипта при клике на землю
    public void BuildTurretOn(Vector3 buildPosition)
    {
        // Если ничего не выбрано для строительства - выходим
        if (turretToBuild == null)
            return;

        // Проверяем деньги еще раз (if (GameManager.instance.money < crossbowCost) return;)

        // Создаем турель на сцене
        GameObject newTurret = Instantiate(turretToBuild, buildPosition, Quaternion.identity);
        Debug.Log("Турель построена!");

        // Вычитаем деньги (GameManager.instance.money -= crossbowCost;)
        GameManager.instance.money -= crossbowCost;
        // Обновляем UI
        UpdateUI();

        // Выходим из режима строительства
        CancelTurretSelection();
    }

    void UpdateUI()
    {
        // Обновляем текст кнопок, их доступность и т.д.
        hasEnoughMoney = (GameManager.instance.money >= crossbowCost);
        buildCrossbowButton.interactable = hasEnoughMoney;
    }
}