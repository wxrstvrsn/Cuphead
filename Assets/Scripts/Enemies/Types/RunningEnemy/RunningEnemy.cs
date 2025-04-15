using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Бегущий противник: активируется, когда игрок в радиусе,
/// бежит в его сторону, прыгает через препятствия и деактивируется при удалении.
/// </summary>
public class RunningEnemy : Enemy
{
    [Header("AI Parameters")] [SerializeField]
    private Transform _player;

    // [SerializeField] private float _activationRadius = 5f;
    [SerializeField] private float _destroyDistance = 15f;
    [SerializeField] private float _jumpDistance = 1.2f;
    [SerializeField] private float _spawnCooldown;
    [SerializeField] private float _cliffDetectionOffset;

    public float GetSpawnCooldown() => _spawnCooldown;

    private EnemyAnimation _enemyAnimation;

    private Vector3 _spawnPoint;

    private bool _isActive = false;
    private float _direction = -1f;
    private float _cooldownTimer;

    protected override void Awake()
    {
        base.Awake();
        _enemyAnimation = GetComponent<EnemyAnimation>();
        _spawnPoint = transform.position;


        if (_player == null)
            Debug.LogWarning("RunningEnemy: ссылка на игрока не установлена!");
    }

    private void Update()
    {
        if (_player == null) return;

        if (!_isActive)
        {
            TryActivate();
            return;
        }

        HandleAI();
        _enemyAnimation.SetGrounded(IsGrounded() || IsGroundedOnWall());

        // Проверка выхода за пределы
        if (Mathf.Abs(transform.position.x - _player.position.x) > _destroyDistance)
        {
            Deactivate();
        }
    }

    /// <summary>
    /// AI-поведение: движение и прыжки через препятствия
    /// </summary>
    private void HandleAI()
    {
        bool grounded = IsGrounded() || IsGroundedOnWall();

        if ((IsObstacleAhead() || IsCliffAhead())  && (IsGrounded() || IsGroundedOnWall()))
        {
            print("RUNNER TRYNA JUMP");
            Jump();
            _enemyAnimation.PlayJump();
        }

        if (grounded)
        {
            Move(_direction);
        }
    }

    /// <summary>
    /// Проверка на наличие препятствия впереди
    /// </summary>
    private bool IsObstacleAhead()
    {
        CapsuleCollider2D collider = GetCapsuleCollider;
        Vector2 center = collider.bounds.center;
        center.x += _direction * collider.bounds.extents.x * 0.9f;

        Vector2 size = collider.bounds.size;
        size.y *= 0.3f;
        size.x *= 0.9f;

        float distance = _jumpDistance;

        RaycastHit2D hit = Physics2D.BoxCast(center, size, 0f, Vector2.right * _direction, distance, wallLayer);

        // Визуализация (только в редакторе, не влияет на игру)
        Color rayColor = hit.collider != null ? Color.red : Color.green;
        Debug.DrawRay(center, Vector2.right * (_direction * distance), rayColor);
        /*
        |---------------|------------------------------------------------|
        | 🔴луч         |Есть столкновение (прыгать будем)               |
        |---------------|------------------------------------------------|
        | 🟢луч         |Всё чисто, можно бежать                         |
        |---------------|------------------------------------------------|
        |🟡прямоугольник|Размер BoxCast — насколько большой хитбокс врага|
        |---------------|------------------------------------------------|
        */


        // Также отрисуем размер box'a (прямоугольник)
        Vector3 center3D = new Vector3(center.x, center.y, 0f);
        Vector3 halfSize = new Vector3(size.x, size.y, 0f) * 0.5f;

        Debug.DrawLine(center3D - halfSize, center3D + halfSize, Color.yellow);

        if (hit.collider != null)
            Debug.Log($"[RunningEnemy] Обнаружено препятствие: {hit.collider.gameObject.name}");

        return hit.collider != null;
    }


    private bool IsCliffAhead()
    {
        Vector2 origin = _body.position + Vector2.right * _direction * _cliffDetectionOffset; // немного впереди врага
        float rayLength = 6.0f;

        RaycastHit2D hitGround = Physics2D.Raycast(origin, Vector2.down, rayLength, groundLayer);
        RaycastHit2D hitWall = Physics2D.Raycast(origin, Vector2.down, rayLength, wallLayer);

        Debug.DrawRay(origin, Vector2.down * rayLength, hitGround.collider ? Color.green : Color.magenta);

        return (hitGround.collider == null) && (hitWall.collider == null); // если не нашли землю — это обрыв
    }


    /// <summary>
    /// Активация по событию ObjectPool или вручную
    /// </summary>
    public override void Activate()
    {
        transform.position = _spawnPoint;
        _isActive = true;
        _direction = Mathf.Sign(_player.position.x - transform.position.x);
        _cooldownTimer = 0f;
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Автоматическая активация по расстоянию
    /// </summary>
    public void TryActivate()
    {
        if (Vector2.Distance(transform.position, _player.position) <= _activationRadius)
        {
            _isActive = true;
            _direction = Mathf.Sign(_player.position.x - transform.position.x);
        }
    }

    /// <summary>
    /// Выключение врага и возврат в начальное положение
    /// </summary>
    public override void Deactivate()
    {
        _isActive = false;
        transform.position = _spawnPoint;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_isActive) return;

        if (other.CompareTag("Player"))
        {
            Debug.Log("💥 Игрок получил урон от RunningEnemy");
            // TODO: Вызов GetDamage() у игрока
        }
    }
}