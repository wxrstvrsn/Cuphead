using UnityEngine;

public class CameraTargetFollow : MonoBehaviour
{
    [SerializeField] private Transform player;      // игрок
    [SerializeField] private float yThreshold = 1f; // насколько выше/ниже должен прыгнуть
    [SerializeField] private float verticalSmoothTime = 0.2f;
    [SerializeField] private float horizontalSmoothTime = 0.05f;

    private Vector3 velocity = Vector3.zero;
    private float targetY;
    private bool initialized = false;

    private void LateUpdate()
    {
        if (!player) return;

        Vector3 targetPosition = transform.position;

        // Следим по X всегда
        targetPosition.x = Mathf.SmoothDamp(transform.position.x, player.position.x, ref velocity.x, horizontalSmoothTime);

        // Первичная инициализация Y
        if (!initialized)
        {
            targetY = player.position.y;
            targetPosition.y = targetY;
            initialized = true;
        }

        // Обновляем Y только если игрок ушёл выше или ниже порога
        if (Mathf.Abs(player.position.y - targetY) > yThreshold)
        {
            targetY = player.position.y;
        }

        // Плавно догоняем по Y (если обновили)
        targetPosition.y = Mathf.SmoothDamp(transform.position.y, targetY, ref velocity.y, verticalSmoothTime);

        // Обновляем позицию цели камеры
        transform.position = targetPosition;
    }
}