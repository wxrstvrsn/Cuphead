using UnityEngine;

public class SmartCameraZoneFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector2 followZoneSize = new Vector2(4f, 2f); // ширина и высота зоны слежения (в мировых координатах)
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private float minY = 0f;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        if (target == null)
            Debug.LogError("SmartCameraZoneFollow: Target не задан!");
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 cameraPos = transform.position;
        Vector3 targetPos = target.position;

        // Горизонтальное слежение всегда
        float leftBound = cameraPos.x - followZoneSize.x / 2f;
        float rightBound = cameraPos.x + followZoneSize.x / 2f;

        if (targetPos.x < leftBound)
            cameraPos.x = targetPos.x + followZoneSize.x / 2f;
        else if (targetPos.x > rightBound)
            cameraPos.x = targetPos.x - followZoneSize.x / 2f;

        // Вертикальное слежение — только если игрок вышел за "рамку"
        float bottomBound = cameraPos.y - followZoneSize.y / 2f;
        float topBound = cameraPos.y + followZoneSize.y / 2f;

        if (targetPos.y < bottomBound)
            cameraPos.y = targetPos.y + followZoneSize.y / 2f;
        else if (targetPos.y > topBound)
            cameraPos.y = targetPos.y - followZoneSize.y / 2f;

        // Ограничение по минимальной высоте
        cameraPos.y = Mathf.Max(cameraPos.y, minY);

        // Плавное движение
        transform.position = Vector3.Lerp(transform.position, cameraPos, Time.deltaTime * smoothSpeed);
    }

    // Гизмо для отображения зоны слежения в редакторе
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(followZoneSize.x, followZoneSize.y, 0));
    }
}
