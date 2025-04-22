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

        // Через секунду прицеливаемся в игрока
        Invoke(nameof(StartHoming), 2f);
    }


    private void StartHoming()
    {
        if (_target == null)
        {
            Debug.LogWarning("Нет цели для наведения у морковки!");
            return;
        }

        Vector2 toTarget = (_target.position - transform.position);

        // Проверка: не лететь "вверх", если игрок уже над нами
        if (toTarget.y > 0)
        {
            Debug.Log("Цель выше пули, сохраняем направление вниз.");
            return; // не меняем направление
        }

        _direction = toTarget.normalized;

        // Для красоты: морковка поворачивается в сторону
        transform.up = _direction;
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