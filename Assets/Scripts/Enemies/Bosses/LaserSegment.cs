using UnityEngine;

public class LaserSegment : MonoBehaviour
{
    private Vector2 _direction;
    private float _speed = 6f;
    private float _lifetime = 3f;
    private float _timer;

    public void Activate(Vector2 direction)
    {
        _direction = direction.normalized;
        _timer = 0f;
        gameObject.SetActive(true);
        CancelInvoke(); // на всякий
        Invoke(nameof(Deactivate), _lifetime);
    }

    private void Update()
    {
        transform.position += (Vector3)(_direction * (_speed * Time.deltaTime));
        _timer += Time.deltaTime;

        if (_timer >= _lifetime)
        {
            Deactivate();
        }
    }

    private void Deactivate()
    {
        _timer = 0f;
        gameObject.SetActive(false);
    }
}