using System;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Скрипт обработки перемещений персонажа игрока
/// </summary>
public class Player : Entity
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

    protected override void Awake()
    {
        base.Awake();
        _playerAnim = GetComponent<PlayerAnimation>();
    }

    private void Update()
    {
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

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Dash();

            _playerAnim.PlayDash();
        }

        Move(horizontalInput);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Jump();

            _playerAnim.PlayJump();
        }

        _playerAnim.SetRunning(isMoving);
    }

    private void Dash()
    {
        float direction = Mathf.Sign(_body.transform.localScale.x);
        _body.linearVelocity = new Vector2(dashForce * direction, _body.linearVelocity.y);
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