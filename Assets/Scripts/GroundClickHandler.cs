using UnityEngine;

public class GroundClickHandler : MonoBehaviour
{
    private Camera mainCamera;
    public LayerMask groundLayer; // ������ � ������� ���� "Ground" � Unity ��� ������������, ���� ����� ������� ������

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // ���� ���� �������� � ������� ������ ��� ���������
        if (Input.GetMouseButtonDown(0) && BuildManager.instance.selectedTurret != null)
        {
            // ������� ��� �� ������ � ����� �����
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, groundLayer);

            // ���� ��� ����� � ����� (������ �� ���� Ground)
            if (hit.collider != null)
            {
                Debug.Log("����� ������� ���: " + hit.point);
                // ������ BuildManager ��������� ������ � ���� �����
                BuildManager.instance.BuildTurretOn(hit.point);
            }
            else
            {
                Debug.Log("����� ������� ������!");
            }
        }
    }
}