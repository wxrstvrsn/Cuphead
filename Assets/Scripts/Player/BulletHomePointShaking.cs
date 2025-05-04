using UnityEngine;

[RequireComponent(typeof(Transform))]
public class BulletHomePointShaking : MonoBehaviour
{
    [Header("Амплитуда покачивания (в юнитах)")] [SerializeField]
    private float amplitude = 0.05f;

    [Header("Частота покачивания (кол–во колебаний в секунду)")] [SerializeField]
    private float frequency = 5f;

    [SerializeField] private Vector2 direction;

    private Vector3 _originalLocalPos;

    private void Awake()
    {
        _originalLocalPos = transform.localPosition;
    }

    private void Update()
    {
        // Расчитываем текущее смещение по синусу
        float wave = Mathf.Sin(Time.time * frequency * Mathf.PI * 2f) * amplitude;

        if (direction.x == 1)
        {
            transform.localPosition = _originalLocalPos + new Vector3(0f, wave, 0f);
        }

        if (direction.y == 1)
        {
            transform.localPosition = _originalLocalPos + new Vector3(wave, 0f, 0f);
        }
    }
}