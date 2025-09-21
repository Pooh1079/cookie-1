using UnityEngine;
using UnityEngine.UI; // ����� ��� ������ � UI ����������

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;

    [Header("������� �������")]
    public GameObject crossbowPrefab; // ���������� ���� ������ Prefab_Crossbow

    [Header("��������� �������")]
    public int crossbowCost = 100;

    [Header("������ �� UI")]
    public GameObject buildMenuPanel; // ������ � ������� ������
    public Button buildCrossbowButton; // ���������� ������ "��������� �������"
    public Button cancelButton; // ������ ������
    public Button buildModeButton; // ������� ������ "Build"

    // ����� ������ ������� ��� ���������?
    public GameObject turretToBuild;
    // ���������� �� �����?
    private bool hasEnoughMoney = false;

    void Awake()
    {
        // ������ �������� ��������� (Singleton)
        if (instance != null)
        {
            Debug.LogError("������ ������ BuildManager �� �����!");
            return;
        }
        instance = this;
    }

    void Start()
    {
        // ��������� ��������� ��� ������
        UpdateUI();

        // ����� �������� ���� ������ �������
        if (buildMenuPanel != null)
            buildMenuPanel.SetActive(false);

        // ������ ������ ������ ����������
        if (cancelButton != null)
            cancelButton.gameObject.SetActive(false);
    }

    void Update()
    {
        // ��������� ���������, ������� �� ����� �� ��������� ������
        // (����� ��������������, ��� � GameManager ���� ���������� 'money')
         hasEnoughMoney = (GameManager.instance.money >= crossbowCost);

        // ���� ������ ������� � �� � ������ �������������, ����� �������� ���������� ���������
        // (��������, �������������� ������� ������, ��������� �� ��������)
        if (turretToBuild != null)
        {
            // ������ ����������� "��������" ������
        }

        // ��������� ������ ������������� �� ������ ������ ���� ��� ESC
        if (turretToBuild != null && (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape)))
        {
            CancelTurretSelection();
        }
    }

    // ���������� �� ����� �� ������� ������ "Build"
    public void ToggleBuildMenu()
    {
        // �������� ��� ��������� ���� ������ �������
        bool isMenuActive = buildMenuPanel.activeSelf;
        buildMenuPanel.SetActive(!isMenuActive);

        // ���� �� ������� ����, ������� �� ������ �������������
        if (!isMenuActive && turretToBuild != null)
        {
            CancelTurretSelection();
        }
    }

    // ���������� �� ����� �� ������ "��������� �������"
    public void SelectTurretToBuild()
    {
        // 1. ��������� ������ (if (!hasEnoughMoney) return;)
        // 2. ������� ������
        turretToBuild = crossbowPrefab;
        Debug.Log("������� ������ ��� ���������: " + turretToBuild.name);

        // 3. ������� ���� ������
        buildMenuPanel.SetActive(false);

        // 4. ������������ ������ ������
        cancelButton.gameObject.SetActive(true);

        // 5. ����� ����� �������� ���������� ��������� (������� ������)
    }

    // ���������� �� ����� �� ������ "Cancel" ��� ������ ������� ����
    public void CancelTurretSelection()
    {
        turretToBuild = null;
        Debug.Log("����� ������������� �������.");

        // ������������ ������ ������
        cancelButton.gameObject.SetActive(false);

        // �������� ���������� ���������
    }

    // ���� ����� ����� ���������� �� ������� ������� ��� ����� �� �����
    public void BuildTurretOn(Vector3 buildPosition)
    {
        // ���� ������ �� ������� ��� ������������� - �������
        if (turretToBuild == null)
            return;

        // ��������� ������ ��� ��� (if (GameManager.instance.money < crossbowCost) return;)

        // ������� ������ �� �����
        GameObject newTurret = Instantiate(turretToBuild, buildPosition, Quaternion.identity);
        Debug.Log("������ ���������!");

        // �������� ������ (GameManager.instance.money -= crossbowCost;)
        GameManager.instance.money -= crossbowCost;
        // ��������� UI
        UpdateUI();

        // ������� �� ������ �������������
        CancelTurretSelection();
    }

    void UpdateUI()
    {
        // ��������� ����� ������, �� ����������� � �.�.
        hasEnoughMoney = (GameManager.instance.money >= crossbowCost);
        buildCrossbowButton.interactable = hasEnoughMoney;
    }
}