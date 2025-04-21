using System;
using UnityEngine;

/// <summary>
/// Патрулирующий враг: двигается между двумя точками, останавливается при получении урона
/// </summary>
public class PatrollingEnemy : Enemy, IDamageable
{
    [Header("Границы патрулируемой зоны")] [SerializeField]
    private Transform _startPoint;

    [SerializeField] private Transform _endPoint;

    [Header("Время, которое Патрулянт неактивен")] [SerializeField]
    private float _disableTime;

    
    private float _disableTimer;
    private float _direction = 1;
    /*private bool _isActive;*/
    private bool _canDealDamage;
    private CapsuleCollider2D _col;


    protected override void Awake()
    {
        base.Awake();

        _col = GetComponent<CapsuleCollider2D>();
        _canDealDamage = true;
        _isActive = true;

        _enemyAnimation = GetComponent<EnemyAnimation>();

        if (_enemyAnimation == null)
            Debug.LogError("EnemyAnimation не найден на " + gameObject.name);
    }

    /* TODO: дописать: пока Патрулянт не
        завершил анимацию PopOut() не начинать двигаться
        и не включать running()  */
    private void Update()
    {
        if (_isActive)
        {
            MoveAI();
        }
        else
        {
            _body.constraints = RigidbodyConstraints2D.FreezeAll;
            _disableTimer += Time.deltaTime;

            if (_enemyAnimation.GetRunning())
                _enemyAnimation.PlayHide();

            if (_disableTimer >= _disableTime)
                Reactivate();
        }

        _enemyAnimation.SetRunning(_isActive);
    }

    /// <summary>
    /// AI-логика патрулирования
    /// </summary>
    private void MoveAI()
    {
        Move(_direction); // метод из Entity

        if (_direction == 1 && transform.position.x + 2.0f >= _endPoint.position.x)
            _direction = -1;
        else if (_direction == -1 && transform.position.x - 2.0f <= _startPoint.position.x)
            _direction = 1;
    }

    /**/
    // 
    /// <summary>
    /// Получение урона от пули
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet") && _canDealDamage)
        {
            _col.enabled = false;
            _body.constraints = RigidbodyConstraints2D.FreezeAll;
            _isActive = false;
            _canDealDamage = false;
            _disableTimer = 0f;
        }
        // костыль с stackOverFlow
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
    }

    /// <summary>
    /// Возвращает врага к активности
    /// </summary>
    private void Reactivate()
    {
        _enemyAnimation.PlayPopOut();

        _col.enabled = true;
        _isActive = true;

        _body.constraints = RigidbodyConstraints2D.FreezeRotation;

        _canDealDamage = true;
        _disableTimer = 0f;
    }

    public override void Activate()
    {
        base.Activate();

        _disableTimer = 0f;
    }

    public override void Deactivate()
    {
        base.Deactivate();
        
        gameObject.SetActive(false);
    }

    public void GetDamage()
    {
        
    }
}