using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class PatrollingEnemy : Enemy
{
    private Rigidbody2D _bodyPatrolling;
    private State _currentState;
    [SerializeField] private float _disableTime;
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Transform _endPoint;

    private EnemyAnimation _enemyAnimation;
    private bool _isActive = true;
    private float _disableTimer;
    private float _direction = 1;
    
    private void Start()
    {
        _isActive = true;
        _direction = 1;
    }


    protected override void Awake()
    {
        base.Awake();
        _enemyAnimation = GetComponent<EnemyAnimation>();
        _bodyPatrolling = GetComponent<Rigidbody2D>();

        if (_enemyAnimation == null)
            Debug.LogError("PatrollingEnemy: NULL REFERENCE");
    }

    // Update is called once per frame
    void Update()
    {
        /*if (!_isActive)
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
        }*/

        // if (_isActive)
        {
            print($"PatrollingEnemy tryna moving!!! DIRECTION: {_direction}");
            MoveAI();
            Debug.Log("Velocity: " + _bodyPatrolling.linearVelocity);

        }

        // _enemyAnimation.SetRunning(_isActive);
    }


    private void MoveAI()
    {
        /*Move(_direction);*/
        /*print("MOVEAI: MOVE REQUESTED");*/

        /*_direction = (_direction == 1 && transform.position.x >= _endPoint.position.x) ? -1 : 0f;
        _direction = (_direction == -1 && transform.position.x <= _startPoint.position.x) ? 1 : 0f;*/

        /*if (_direction == 1 && transform.position.x >= _endPoint.position.x)
        {
            _direction = -1;
        }
        else if (_direction == -1 && transform.position.x <= _startPoint.position.x)
        {
            _direction = 1;
        }*/
        print("--------------HERE------------");
        _bodyPatrolling.linearVelocity = new Vector2(_direction * speed, _bodyPatrolling.linearVelocity.y);
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