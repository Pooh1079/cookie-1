using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 targetPosition;
    private bool isMoving;
    public GameObject moveIndicator;

    void Update()
    {
        // �������� 1: ������� �� ������ �� UI �������?
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        // �������� 2: ������� �� ����� �������������? (������������ ������)
        if (BuildManager.instance != null && BuildManager.instance.selectedTurret != null)
        {
            return;
        }

        // ��������� ����� ��� �����������
        if (Input.GetMouseButtonDown(0))
        {
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            isMoving = true;

            if (moveIndicator != null)
            {
                Instantiate(moveIndicator, targetPosition, Quaternion.identity);
            }
        }

        if (isMoving)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            if ((Vector2)transform.position == targetPosition)
            {
                isMoving = false;
            }
        }
    }
}