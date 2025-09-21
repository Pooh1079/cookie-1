using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Настройки")]
    public Transform target;        // Объект за которым следует камера (игрок)
    public float smoothSpeed = 0.125f; // Плавность движения
    public Vector3 offset;         // Смещение камеры относительно игрока

    void LateUpdate()
    {
        if (target == null) return;

        // Вычисляем желаемую позицию
        Vector3 desiredPosition = target.position + offset;

        // Плавное перемещение
        Vector3 smoothedPosition = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed
        );

        // Устанавливаем позицию камеры
        transform.position = smoothedPosition;
    }
}
