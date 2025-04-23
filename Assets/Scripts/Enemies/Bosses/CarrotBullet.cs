using UnityEngine;

public class CarrotBullet : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _homingDelay = 1f;
    [SerializeField] private float _lifetime = 6f;

    [Header("References")]
    [SerializeField] private Transform _visual;

    private Transform _target;
    private Vector2 _direction;
    private bool _isFlying;
    private float _timer;

    public void Launch(Transform target)
    {
        _target = target;
        _direction = Vector2.down; // падаем вниз сначала
        _isFlying = true;
        _timer = 0f;

        gameObject.SetActive(true);

        // через _homingDelay секунды переключаемся на прицеливание
        Invoke(nameof(StartHoming), _homingDelay);
    }

    private void StartHoming()
    {
        if (_target == null) return;

        _direction = (_target.position - transform.position).normalized;

        // Вращаем визуал по направлению (_direction) — коллинеарно!
        float angle = Vector2.SignedAngle(Vector2.down, _direction);
        _visual.rotation = Quaternion.Euler(0f, 0f, angle);
    }


    private void Update()
    {
        if (!_isFlying) return;

        transform.position += (Vector3)(_direction * (_speed * Time.deltaTime));

        _timer += Time.deltaTime;
        if (_timer >= _lifetime || transform.position.y < -30f)
        {
            Deactivate();
        }
    }

    public void Deactivate()
    {
        _isFlying = false;
        CancelInvoke();

        // Сброс поворота спрайта
        _visual.rotation = Quaternion.identity;

        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            if (other.CompareTag("Player"))
            {
                damageable.GetDamage();
                Deactivate();
            }

        }
        else if (other.CompareTag("PlayerBullet"))
        {
            Deactivate();
        }
    }
}
