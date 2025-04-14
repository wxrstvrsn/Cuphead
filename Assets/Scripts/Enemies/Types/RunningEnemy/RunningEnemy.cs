using System;
using Unity.VisualScripting;
using UnityEngine;

// класс для врагов, object pooling позже
public class RunningEnemy : Enemy
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _speed;
    [SerializeField] private float _timeToLive;
    [SerializeField] private float _destroyDistance;

    /// <summary>
    /// Расстояние до игрока для триггеринга активации противника
    /// </summary>
    [SerializeField] private float _activationRadius;

    /// <summary>
    /// Геттер (испол-ся в PollManager) расстояния для активации противника 
    /// </summary>
    /// <returns></returns>
    public float GetActivationRadius() => _activationRadius;


    private bool _isActive;
    private float _direction;
    private float _timer;

    public float GetTimeToLive() => _timeToLive;

    private void Awake()
    {
        _isActive = false;
    }

    private void Update()
    {
        if (!_isActive || _player == null) return;

        Move(_direction);

        if (Mathf.Abs(transform.position.x - _player.position.x) > _destroyDistance)
        {
            Deactivate();
        }
    }


    public override void Activate()
    {
        if (_player == null)
        {
            Debug.LogWarning("RunningEnemy: reference to target (player) is null.");
        }

        _direction = Mathf.Sign(_player.position.x - transform.position.x);
        _isActive = true;
        gameObject.SetActive(true);
    }

    public override void Deactivate()
    {
        _isActive = false;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_isActive) return;

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Damaged!!!");
            // TODO: GetDamage()
        }
    }
}