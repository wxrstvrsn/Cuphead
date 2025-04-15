using UnityEngine;

/// <summary>
/// Патрулирующий враг: двигается между двумя точками, останавливается при получении урона
/// </summary>
public class PatrollingEnemy : Enemy
{
    [Header("Патрулирование")] [SerializeField]
    private Transform _startPoint;

    [SerializeField] private Transform _endPoint;
    [SerializeField] private float _disableTime = 2f;

    private EnemyAnimation _enemyAnimation;
    private float _disableTimer;
    private float _direction = 1;
    private bool _isActive = true;

    protected override void Awake()
    {
        base.Awake();
        _enemyAnimation = GetComponent<EnemyAnimation>();

        if (_enemyAnimation == null)
            Debug.LogError("EnemyAnimation не найден на " + gameObject.name);
    }

    private void Update()
    {
        if (_isActive)
        {
            MoveAI();
        }
        else
        {
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

    /// <summary>
    /// Получение урона от пули
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            _isActive = false;
            _disableTimer = 0f;
        }
    }

    /// <summary>
    /// Возвращает врага к активности
    /// </summary>
    private void Reactivate()
    {
        _isActive = true;
        _enemyAnimation.PlayPopOut();
        _disableTimer = 0f;
    }

    public override void Activate()
    {
        gameObject.SetActive(true);
        _isActive = true;
        _direction = 1;
        _disableTimer = 0f;
    }

    public override void Deactivate()
    {
        gameObject.SetActive(false);
    }
}