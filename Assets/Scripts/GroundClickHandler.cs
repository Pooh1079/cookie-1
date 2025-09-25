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
        // УЛУЧШЕННАЯ ПРОВЕРКА: проверяем не только что турель выбрана, но и что у нее есть префаб
        if (Input.GetMouseButtonDown(0) && BuildManager.instance != null)
        {
            // Проверяем ВСЕ условия
            bool canBuild = BuildManager.instance.selectedTurret != null &&
                           BuildManager.instance.selectedTurret.prefab != null;

            if (canBuild)
            {
                Debug.Log("Режим строительства активен, строим турель...");

                // Простой способ для 2D
                Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;

                BuildManager.instance.BuildTurretOn(mousePos);
            }
            else
            {
                // Если режим строительства не активен, но selectedTurret не null - это ошибка
                if (BuildManager.instance.selectedTurret != null && BuildManager.instance.selectedTurret.prefab == null)
                {
                    Debug.LogError("ОШИБКА: Выбрана турель без префаба! Имя турели: '" +
                                  BuildManager.instance.selectedTurret.name + "'");
                }
            }
        }
    }
}