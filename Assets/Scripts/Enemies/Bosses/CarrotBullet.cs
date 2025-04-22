using UnityEngine;

public class CarrotBullet : MonoBehaviour
{
    private Transform _target;
    private Vector2 _direction;
    private float _speed = 5f;
    private bool _isFlying;

    public void Launch(Transform target)
    {
        _target = target;
        _direction = Vector2.down;
        _isFlying = true;
        gameObject.SetActive(true);

        // Через секунду меняем траекторию
        Invoke(nameof(StartHoming), 1f);
    }

    private void StartHoming()
    {
        if (_target == null) return;
        _direction = (_target.position - transform.position).normalized;
    }

    private void Update()
    {
        if (!_isFlying) return;

        transform.position += (Vector3)(_direction * _speed * Time.deltaTime);

        if (transform.position.y < -30f)
            Deactivate();
    }

    public void Deactivate()
    {
        _isFlying = false;
        CancelInvoke();
        gameObject.SetActive(false);
    }
}