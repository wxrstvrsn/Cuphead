using System;

using UnityEngine;
using UnityEngine.Serialization;


/// <summary>
/// Скрипт обработки перемещений персонажа игрока
/// </summary>
public class Player : Entity, IDamageable
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

    protected override void Awake()
    {
        base.Awake();
        _playerAnim = GetComponent<PlayerAnimation>();
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
        // Jump();
        _body.linearVelocity = new Vector2(_body.linearVelocity.x, jumpForce * 2);
        _playerAnim.PlayHit();
    }
}
