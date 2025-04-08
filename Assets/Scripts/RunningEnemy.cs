using System;
using Unity.VisualScripting;
using UnityEngine;

// класс для врагов, object pooling позже
public class RunningEnemy : Entity
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _speed;
    [SerializeField] private float _timeToLive;
    [SerializeField] private float _destroyDistance;
    
    private bool _isActive;
    private float _direction;

    public float GetTimeToLive() => _timeToLive;

    private void Awake()
    {
        _isActive = false;
        _direction = Mathf.Sign(_player.position.x - transform.position.x);
    }

    private void Update()
    {
        if(!_isActive || _player == null) return;
        
        Move(_direction);

        if (Mathf.Abs(transform.position.x - _player.position.x) > _destroyDistance)
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
        if(!_isActive) return;

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Damaged!!!");
            // TODO: getDamage in Player
        }
    }

    public void SetDirection(){}


}
