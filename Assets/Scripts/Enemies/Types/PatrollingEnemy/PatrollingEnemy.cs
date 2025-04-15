using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class PatrollingEnemy : Enemy
{
    private State _currentState;
    [SerializeField] private float _disableTime;
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Transform _endPoint;

    private EnemyAnimation _enemyAnimation;
    private bool _isActive;
    private float _disableTimer;
    private float _direction;

    protected override void Awake()
    {
        base.Awake();
        _enemyAnimation = GetComponent<EnemyAnimation>();
        
        if(_enemyAnimation == null)
            Debug.LogError("PatrollingEnemy: NULL REFERENCE");
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isActive)
        {
            _disableTimer += Time.deltaTime;
            if (_enemyAnimation.GetRunning())
            {
                _enemyAnimation.PlayHide();
            }
        }

        if (_disableTimer >= _disableTime)
        {
            if (!_enemyAnimation.GetRunning())
            {
                _enemyAnimation.PlayPopOut();
            }

            _disableTimer = 0;
            _isActive = true;
            MoveAI();
        }

        _enemyAnimation.SetRunning(_isActive);
    }


    private void MoveAI()
    {
        Move(_direction);

        _direction = (_direction == 1 && transform.position.x >= _endPoint.position.x) ? -1 : 0f;
        _direction = (_direction == -1 && transform.position.x <= _startPoint.position.x) ? 1 : 0f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            _isActive = false;
        }
    }

    public override void Activate()
    {
        gameObject.SetActive(true);
    }

    public override void Deactivate()
    {
        gameObject.SetActive(false);
    }
}