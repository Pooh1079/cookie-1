using UnityEngine;

public class GroundClickHandler : MonoBehaviour
{
    private Camera mainCamera;
    public LayerMask groundLayer; // Создай и настрой слой "Ground" в Unity для поверхностей, куда можно ставить турели

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Если мышь кликнула и выбрана турель для постройки
        if (Input.GetMouseButtonDown(0) && BuildManager.instance.selectedTurret != null)
        {
            // Пускаем луч из камеры в точку клика
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, groundLayer);

            // Если луч попал в землю (объект на слое Ground)
            if (hit.collider != null)
            {
                Debug.Log("Можно строить тут: " + hit.point);
                // Просим BuildManager построить турель в этой точке
                BuildManager.instance.BuildTurretOn(hit.point);
            }
            else
            {
                Debug.Log("Здесь строить нельзя!");
            }
        }
    }
}