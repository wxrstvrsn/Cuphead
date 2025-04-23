using UnityEngine;

public class LaserSegment : MonoBehaviour
{
    [SerializeField] private float _speed = 6f;
    [SerializeField] private float _lifetime = 3f;

    private Vector2 _direction;
    private float _timer;
    private bool _isActive;

    public void Launch(Vector2 direction)
    {
        _direction = direction.normalized;
        _timer = 0f;
        _isActive = true;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!_isActive) return;

        transform.position += (Vector3)(_direction * (_speed * Time.deltaTime));
        _timer += Time.deltaTime;

        if (_timer >= _lifetime || transform.position.y < -30f)
        {
            Deactivate();
        }
    }

    private void Deactivate()
    {
        _isActive = false;
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