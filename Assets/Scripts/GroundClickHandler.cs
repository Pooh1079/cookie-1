using UnityEngine;

public class GroundClickHandler : MonoBehaviour
{
    private Camera mainCamera;
    public LayerMask groundLayer;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // ���������� ��������: ��������� �� ������ ��� ������ �������, �� � ��� � ��� ���� ������
        if (Input.GetMouseButtonDown(0) && BuildManager.instance != null)
        {
            // ��������� ��� �������
            bool canBuild = BuildManager.instance.selectedTurret != null &&
                           BuildManager.instance.selectedTurret.prefab != null;

            if (canBuild)
            {
                Debug.Log("����� ������������� �������, ������ ������...");

                // ������� ������ ��� 2D
                Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;

                BuildManager.instance.BuildTurretOn(mousePos);
            }
            else
            {
                // ���� ����� ������������� �� �������, �� selectedTurret �� null - ��� ������
                if (BuildManager.instance.selectedTurret != null && BuildManager.instance.selectedTurret.prefab == null)
                {
                    Debug.LogError("������: ������� ������ ��� �������! ��� ������: '" +
                                  BuildManager.instance.selectedTurret.name + "'");
                }
            }
        }
    }
}