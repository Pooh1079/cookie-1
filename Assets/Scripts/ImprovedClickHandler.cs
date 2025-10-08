using UnityEngine;
using UnityEngine.EventSystems;

public class ImprovedClickHandler : MonoBehaviour
{
    private Camera mainCamera;
    public LayerMask groundLayer;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && BuildManager.instance != null)
        {
            // Игнорируем клики по UI элементам
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("Клик по UI - игнорируем");
                return;
            }

            // Проверяем, что выбрана турель для строительства
            if (BuildManager.instance.selectedTurret != null &&
                BuildManager.instance.selectedTurret.prefab != null)
            {
                Debug.Log("=== ПЫТАЕМСЯ ПОСТРОИТЬ ТУРЕЛЬ ===");
                Debug.Log("Выбрана турель: " + BuildManager.instance.selectedTurret.name);

                Vector3 mousePos = Input.mousePosition;
                Ray ray = mainCamera.ScreenPointToRay(mousePos);

                Debug.Log("Mouse position: " + mousePos);
                Debug.Log("Ray origin: " + ray.origin);
                Debug.Log("Ground layer mask: " + groundLayer.value);

                // ДВА ВАРИАНТА РЕЙКАСТА - пробуем оба

                //  c спроверкой
                //RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, groundLayer);

                // без проверки
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

                // Альтернативный вариант для 2D
                Vector2 worldPoint = mainCamera.ScreenToWorldPoint(mousePos);
                RaycastHit2D hit2D = Physics2D.Raycast(worldPoint, Vector2.zero, Mathf.Infinity, groundLayer);

                Debug.Log("3D Raycast hit: " + (hit.collider != null ? hit.collider.name : "null"));
                Debug.Log("2D Raycast hit: " + (hit2D.collider != null ? hit2D.collider.name : "null"));

                if (hit.collider != null || hit2D.collider != null)
                {
                    Vector3 buildPos = hit.collider != null ? hit.point : (Vector3)hit2D.point;
                    Debug.Log("Можно строить тут: " + buildPos);
                    BuildManager.instance.BuildTurretOn(buildPos);
                }
                else
                {
                    Debug.Log("Здесь строить нельзя! Проверь:");
                    Debug.Log("- Назначен ли слой Ground объектам земли?");
                    Debug.Log("- Правильно ли настроен groundLayer в инспекторе?");
                    Debug.Log("- Есть ли у объектов земли Collider2D?");
                }
            }
            else
            {
                Debug.Log("Не выбрана турель или нет префаба");
            }
        }
    }
}