using UnityEngine;

public class CarrotBullet : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _homingDelay = 1f;
    [SerializeField] private float _lifetime = 6f;

    private Transform _target;
    private Vector2 _direction;
    private bool _isFlying;
    private float _timer;

    public void Launch(Transform target)
    {
        _target = target;
        _direction = Vector2.down; // начальное падение вниз
        _isFlying = true;
        _timer = 0f;

        gameObject.SetActive(true);

        // запланировать смену направления
        Invoke(nameof(StartHoming), _homingDelay);
    }

    private void StartHoming()
    {
        if (_target == null) return;

        _direction = (_target.position - transform.position).normalized;
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
        gameObject.SetActive(false);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.GetDamage();
            Deactivate();
        }
    }

}