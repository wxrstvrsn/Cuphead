using UnityEngine;

public class SmartCameraZoneFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector2 followZoneSize = new Vector2(4f, 2f);
    [SerializeField] private float smoothTime = 0.2f; // Чем меньше, тем быстрее камера реагирует
    [SerializeField] private float minY = 0f;

    private Vector3 velocity = Vector3.zero;

    private void FixedUpdate()
    {
        if (target == null) return;

        Vector3 currentPos = transform.position;
        Vector3 targetPos = target.position;
        Vector3 desiredPos = currentPos;

        // Горизонтальные границы зоны
        float leftBound = currentPos.x - followZoneSize.x / 2f;
        float rightBound = currentPos.x + followZoneSize.x / 2f;

        if (targetPos.x < leftBound)
            desiredPos.x = targetPos.x + followZoneSize.x / 2f;
        else if (targetPos.x > rightBound)
            desiredPos.x = targetPos.x - followZoneSize.x / 2f;

        // Вертикальные границы зоны
        float bottomBound = currentPos.y - followZoneSize.y / 2f;
        float topBound = currentPos.y + followZoneSize.y / 2f;

        if (targetPos.y < bottomBound)
            desiredPos.y = targetPos.y + followZoneSize.y / 2f;
        else if (targetPos.y > topBound)
            desiredPos.y = targetPos.y - followZoneSize.y / 2f;

        // Ограничение по нижней границе
        desiredPos.y = Mathf.Max(desiredPos.y, minY);

        // Плавное движение с SmoothDamp
        Vector3 smoothedPosition = Vector3.SmoothDamp(currentPos, desiredPos, ref velocity, smoothTime);

        transform.position = smoothedPosition;
    }
}