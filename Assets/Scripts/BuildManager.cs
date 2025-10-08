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

    [Header("������ ��������� �������")]
    public TurretBlueprint[] turrets;

    [Header("������ �� UI")]
    public GameObject buildMenuPanel;
    public Button cancelButton;
    public Button buildModeButton;

    // ������� public ��� �������
    [Header("Debug - ������� ������")]
    public TurretBlueprint selectedTurret;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("������ ������ BuildManager �� �����!");
            return;
        }
        instance = this;
    }

    void Start()
    {
        Debug.Log("=== START ===");
        Debug.Log("turrets.Length: " + turrets.Length);
        Debug.Log("selectedTurret: " + (selectedTurret == null ? "null" : selectedTurret.name));

        // ������������� ������ - �����!
        InitializeBuildButtons();

        buildMenuPanel.SetActive(false);
        cancelButton.gameObject.SetActive(false);
    }

    // ����� �����: ������������� ������ �������������
    void InitializeBuildButtons()
    {
        for (int i = 0; i < turrets.Length; i++)
        {
            int index = i; // �����: ������� ��������� ����� ��� ���������
            if (turrets[i].buildButton != null)
            {
                // ������� ������ ����������� � ��������� �����
                turrets[i].buildButton.onClick.RemoveAllListeners();
                turrets[i].buildButton.onClick.AddListener(() => SelectTurret(index));
                Debug.Log($"������ ���������������� ��� ������: {turrets[i].name}, ������: {index}");
            }
            else
            {
                Debug.LogWarning($"� ������ {turrets[i].name} ��� ������ buildButton!");
            }
        }
    }

    void Update()
    {
        // ���������� ���������� ������ ���� (��������)
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
        Debug.Log("�� �������� - selectedTurret: " + (selectedTurret == null ? "null" : selectedTurret.name));

        // ���������� ������ ��� ��������/�������� ����
        selectedTurret = null;

        bool isMenuActive = buildMenuPanel.activeSelf;
        buildMenuPanel.SetActive(!isMenuActive);

        // �������� ������ ������ ��� ��������/�������� ����
        cancelButton.gameObject.SetActive(false);

        // ��������: �������� ��� selectedTurret ������� null
        if (selectedTurret != null)
        {
            Debug.LogError("������: selectedTurret ���� ��-null ����� ������! ���: " + selectedTurret.name);
            selectedTurret = null; // ������������� ����������
        }

        Debug.Log("����� �������� - selectedTurret: " + (selectedTurret == null ? "null" : "NOT NULL"));
        Debug.Log("���� ������: " + (!isMenuActive ? "�������" : "�������"));
    }

    public void SelectTurret(int turretIndex)
    {
        Debug.Log("=== SELECT TURRET ===");
        Debug.Log("������: " + turretIndex);
        Debug.Log("������ �������: " + turrets.Length);

        if (turretIndex < 0 || turretIndex >= turrets.Length)
        {
            Debug.LogError("�������� ������ ������!");
            return;
        }

        selectedTurret = turrets[turretIndex];

        // �������������� �������� �������
        if (selectedTurret.prefab == null)
        {
            Debug.LogError($"� ������ '{selectedTurret.name}' ����������� ������!");
            selectedTurret = null;
            return;
        }

        Debug.Log("������� ������: " + selectedTurret.name + " (������: " + (selectedTurret.prefab != null ? "����" : "�����������") + ")");

        buildMenuPanel.SetActive(false);
        cancelButton.gameObject.SetActive(true);
    }

    public void CancelTurretSelection()
    {
        Debug.Log("=== CANCEL SELECTION ===");
        Debug.Log("�� ������ - selectedTurret: " + (selectedTurret == null ? "null" : selectedTurret.name));

        selectedTurret = null;
        cancelButton.gameObject.SetActive(false);

        Debug.Log("����� ������ - selectedTurret: " + (selectedTurret == null ? "null" : "NOT NULL"));
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

        // �������� �����
        if (GameManager.instance != null && GameManager.instance.money < selectedTurret.cost)
        {
            Debug.Log("������������ ����� ��� �������������!");
            CancelTurretSelection();
            return;
        }

        GameObject newTurret = Instantiate(selectedTurret.prefab, buildPosition, Quaternion.identity);
        Debug.Log("������ ���������!");

        // �������� �����
        if (GameManager.instance != null)
        {
            GameManager.instance.money -= selectedTurret.cost;
            Debug.Log($"������� {selectedTurret.cost} �����. ��������: {GameManager.instance.money}");
        }

        CancelTurretSelection();
    }
}