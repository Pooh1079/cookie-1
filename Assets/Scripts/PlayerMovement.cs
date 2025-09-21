using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 targetPosition;
    private bool isMoving;
    public GameObject moveIndicator; // Префаб индикатора перемещения

    void Start()
    {
        targetPosition = transform.position;
        isMoving = false;
    }

    void Update()
    {

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (BuildManager.instance != null && BuildManager.instance.turretToBuild != null)
        {
            return;
        }
        // При клике левой кнопкой мыши
        if (Input.GetMouseButtonDown(0))
        {
            // Получаем позицию клика в мировых координатах
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            isMoving = true;

            // Создаем индикатор перемещения
            if (moveIndicator != null)
            {
                Instantiate(moveIndicator, targetPosition, Quaternion.identity);
            }
        }

        // Перемещаем персонажа к цели
        if (isMoving)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Если персонаж достиг цели, останавливаемся
            if ((Vector2)transform.position == targetPosition)
            {
                isMoving = false;
            }
        }
    }
}