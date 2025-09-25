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
        // ПРОВЕРКА 1: Наведен ли курсор на UI элемент?
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        // ПРОВЕРКА 2: Активен ли режим строительства? (ИСПРАВЛЕННАЯ СТРОКА)
        if (BuildManager.instance != null && BuildManager.instance.selectedTurret != null)
        {
            return;
        }

        // Обработка клика для перемещения
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