using System;
using UnityEngine;

/// <summary>
/// Скрипт обработки перемещений персонажа игрока
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    // подходящее значение для gravityScale - 5
    [SerializeField] private float speed; // 8
    [SerializeField] private float jumpForce; // 15 
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    private Rigidbody2D body;
    private PlayerAnimation playerAnim;
    private PlayerShooting playerShoot;
    private BoxCollider2D boxCollider;

    private bool isRunning;
    public bool IsRunning() => isRunning;
    public bool IsGrounded() => CheckIsGrounded();

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<PlayerAnimation>();
        playerShoot = GetComponent<PlayerShooting>();
        boxCollider = GetComponent<BoxCollider2D>();
        // playerCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        HadleMovement();
        HandleJump();
        playerAnim.SetGrounded(CheckIsGrounded());
    }

    void HadleMovement()
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
    }
}
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