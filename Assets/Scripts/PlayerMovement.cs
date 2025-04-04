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
    private Animator anim;
    private BoxCollider2D boxCollider;
    
    
    
    
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
    

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        anim.SetBool("shootingStraight", false);
        anim.SetBool("shootingDiagonal", false);
        boxCollider = GetComponent<BoxCollider2D>();
        // playerCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        if (!isWallCollision()) // костыль, чтобы не прилипать к стене, если въебался, будучи в прыжке
        {
            body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);
        }

        // 
        if (horizontalInput > 0.01f)
        {
            transform.localScale = Vector3.one;
        }
        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (Input.GetKey(KeyCode.Space) && isGrounded())
        {
            Jump();
        }

        anim.SetBool("runNormal", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());

        /*if (isWallCollision())
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, -jumpForce);
        }*/
    }

    private void Jump()
    {
        // TODO: add isGrounded checker that removes multiple jumps in air
        // DetectPlatformAbove();
        body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
        // Invoke("EnableCollision", 50f); 
        anim.SetTrigger("jump");
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f,
            Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool isWallCollision()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f,
            new Vector2(transform.localScale.x, 0), 0.3f, wallLayer);
        return raycastHit.collider != null;
    }

    /*void EnableCollision()
    {
        if (oneWayPlatformCollider != null)
        {
            Physics2D.IgnoreCollision(oneWayPlatformCollider, playerCollider, false);
            print("Collision ENABLED");
            oneWayPlatformCollider = null;
        }
    }*/
}