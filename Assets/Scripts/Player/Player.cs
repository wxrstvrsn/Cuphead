using System;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Скрипт обработки перемещений персонажа игрока
/// </summary>
public class Player : Entity, IDamageable
/*Почему это удобно:
Враг не знает, что это "Player" — он просто говорит: "Я вижу, что ты IDamageable — держи по щам".

Работает и с игроком, и с будущими NPC, которых можно дамажить.

Разгружает зависимости, не нужен FindObjectOfType<Player>() или [SerializeField] Player player


Ты запутался, потому что...
Ты думал, что интерфейс нужен для врагов, а на самом деле он нужен тем, кто "принимает урон".*/

/* TODO:
    Добавить в Scene объекты -- deadZone в тех местах на мапе,
    где персонаж может провалиться -- и если OnTriggerEnter2D
    то GetDamage() + SetInvincibilty() (мб в один метод типа завернуть)
    и подкидываем его вверх */
{
    // подходящее значение для gravityScale - 5
    /// <summary>
    /// Экземпляр класса, для управления анимацией персонажа
    /// </summary>
    private PlayerAnimation _playerAnim;

    /// <summary>
    /// Экземпляр класса, для управления механикой стрельбы
    /// </summary>
    private PlayerShooting _playerShoot;

    /// <summary>
    /// Ссылка из Inspector'a на Layer для "коллайдеров-ям"
    /// </summary>
    [SerializeField] private LayerMask deadZoneLayer;

    /// <summary>
    /// Переменная значения силы рывка
    /// </summary>
    [SerializeField] private float dashForce;

    private bool _isDashing = false;
    [SerializeField] private float _dashDuration;
    private float _dashTimer;
    private float _originalGravityScale;

    /// <summary>
    /// Булевая переменная состояния бега                       
    /// </summary>
    private bool _isRunning;

    /// <summary>
    /// Метод-геттер значения переменной состояния бега
    /// </summary>
    /// <returns></returns>
    public bool IsRunning() => _isRunning;

    /*public float GetVelocityX => */

    [SerializeField] private int _healthPoints;

    /// <summary>
    /// Класс-геттер положения игрока
    /// </summary>
    // TODO: а бля, точно, создать ж нельзя объект статического класса
    // за ссылку на метод стат класса выступает уже тупа общее имя а не имя переменной в которой лежит созданный экземпляр (пушто его нет бля)

    protected override void Awake()
    {
        base.Awake();
        _playerAnim = GetComponent<PlayerAnimation>();
        
        // TODO: перечитать ч это 
        PlayerShare.Instance = this;
        
    }

    private void Update()
    {
        /*TODO:
            исправить глитч с дэшем, 
            инфинити дэш нот гуд */
        _playerAnim.SetDashing(_isDashing);

        if (_isDashing)
        {
            _dashTimer -= Time.deltaTime;

            // зануляем скорость по Y
            _body.linearVelocity = new Vector2(_body.linearVelocity.x, 0f);

            if (_dashTimer <= 0f)
            {
                // Если кулдаун рывка прошел -- возвращаем физику и меняем булевую
                _isDashing = false;
                _body.gravityScale = _originalGravityScale;
            }

            // Игнорируем пользовательский ввод пока в рывке 
            return;
        }

        HandleInput();
        UpdateAnimationStates();
    }

    /// <summary>
    /// Обработка управления персонажем
    /// </summary>
    private void HandleInput()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        bool isMoving = horizontalInput != 0;

        Move(horizontalInput);

        if (Input.GetKeyDown(KeyCode.LeftShift))
            Dash();

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Jump();
            _playerAnim.PlayJump();
        }

        _playerAnim.SetRunning(isMoving);
    }

    private void Dash()
    {
        _isDashing = true;
        _dashTimer = _dashDuration;

        _originalGravityScale = _body.gravityScale;

        // Кастим булевую в вещественную шоб потом удобно управлять скоростью по У
        // в зависимости в прыжке рывок чи не 
        float isGroundedMultiplicator = (IsGrounded() ? 1 : 0);

        _body.gravityScale *= isGroundedMultiplicator;
        _body.linearVelocity = new Vector2(_body.linearVelocity.x, _body.linearVelocity.y * isGroundedMultiplicator);

        float direction = Mathf.Sign(transform.localScale.x);
        _body.linearVelocity = new Vector2(dashForce * direction, _body.linearVelocity.y * isGroundedMultiplicator);
    }

    /// <summary>
    /// Обновление параметра Grounded из Animator'a
    /// </summary>
    private void UpdateAnimationStates()
    {
        _playerAnim.SetGrounded(IsGrounded() || IsGroundedOnWall());
    }

    /// <summary>
    /// HandleDeadZone falling
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("DeadZone"))
        {
            _body.linearVelocity = new Vector2(_body.linearVelocity.x, jumpForce * 2);
            _playerAnim.PlayHit();
        }
    }

    public void GetDamage()
    {
        _healthPoints--;
        
        _playerAnim.PlayHit();
        
        
        /////
        Jump();
        Debug.Log("Player damaged");
        ////
        
        
        /*TODO:
            дергаем тут SetInvincibility()
            который еще не написал
            ну типа чтобы после
            получения урона какое-то время игрок
            был неуязвим
            чтоб было время отпрыгнуть типа или ч т такое
            SetInvincibility(); */
    }
}

/*void HadleMovement()
{
    float horizontalInput = Input.GetAxis("Horizontal");

    isRunning = horizontalInput != 0;
    playerAnim.SetRunning(isRunning);

    if (!IsTouchingWall())
    {
        body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);
    }

    Flip(horizontalInput);
}

private void HandleJump()
{
    if (Input.GetKey(KeyCode.Space) && CheckIsGrounded())
    {
        body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
        playerAnim.PlayJump();
    }
}

private bool CheckIsGrounded()
{
    RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f,
        Vector2.down, 0.1f, groundLayer);
    return raycastHit.collider != null;
}

private bool IsTouchingWall()
{
    return Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f,
        Vector2.right * transform.localScale.x, 0.3f, wallLayer);
}

private void Flip(float horizontalInput)
{
    if (horizontalInput > 0.01f)
        transform.localScale = Vector3.one;
    else if (horizontalInput < -0.01f)
        transform.localScale = new Vector3(-1, 1, 1);
}*/

/*
     * TODO:
            Придумать-таки как сука не порвав жопу корректно забороть запрыги
            на тайлы, находясь "типа снизу" от их коллайдера
     */
/*
// SHITCODE IS COMING !!!!!!!! TODO rethink???
[SerializeField] private LayerMask oneWayLayer;
private float raycastDistance = 1000f;
private Collider2D oneWayPlatformCollider;
private Collider2D playerCollider;

void DetectPlatformAbove()
{
    print("DETECTING PLATFORM ABOVE");
    // Выпускаем луч вверх от игрока, чтобы найти платформу
    // RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, raycastDistance, oneWayLayer);
    RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f,
        Vector2.up, 20000f, oneWayLayer);

    if (hit.collider != null)
    {
        print("One way collision DISABLED");
        oneWayPlatformCollider = hit.collider;
        Physics2D.IgnoreCollision(playerCollider, oneWayPlatformCollider, true); // Отключаем коллизию
    }
}
// SHIT ENDS
*/

/*void EnableCollision()
{
    if (oneWayPlatformCollider != null)
    {
        Physics2D.IgnoreCollision(oneWayPlatformCollider, playerCollider, false);
        print("Collision ENABLED");
        oneWayPlatformCollider = null;
    }
}*/