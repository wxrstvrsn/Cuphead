using UnityEngine;

public class SmartCameraZoneFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector2 followZoneSize;
    [SerializeField] private float smoothTime;
    [SerializeField] private float minY;
    [SerializeField] private float minX;

    private Vector3 velocity = Vector3.zero;

    // Смещение зоны влево (0.7 = 70% вправо, 30% влево)
    [Range(-10f, 10f)]
    [SerializeField] private float horizontalBias;

    private void FixedUpdate()
    {
        if (target == null) return;

        Vector3 currentPos = transform.position;
        Vector3 targetPos = target.position;
        Vector3 desiredPos = currentPos;

        // Горизонтальные границы зоны слежения со смещением
        float leftBound = currentPos.x - followZoneSize.x * (1 - horizontalBias);
        float rightBound = currentPos.x + followZoneSize.x * horizontalBias;

        if (targetPos.x < leftBound)
            desiredPos.x = targetPos.x + followZoneSize.x * (1 - horizontalBias);
        else if (targetPos.x > rightBound)
            desiredPos.x = targetPos.x - followZoneSize.x * horizontalBias;

        // Вертикальные границы
        float bottomBound = currentPos.y - followZoneSize.y / 2f;
        float topBound = currentPos.y + followZoneSize.y / 2f;

        if (targetPos.y < bottomBound)
            desiredPos.y = targetPos.y + followZoneSize.y / 2f;
        else if (targetPos.y > topBound)
            desiredPos.y = targetPos.y - followZoneSize.y / 2f;

        // Ограничение по нижней границе
        desiredPos.y = Mathf.Max(desiredPos.y, minY);
        desiredPos.x = Mathf.Max(desiredPos.x, minX);

        // Плавное движение камеры
        Vector3 smoothedPosition = Vector3.SmoothDamp(currentPos, desiredPos, ref velocity, smoothTime);
        transform.position = smoothedPosition;
    }
}