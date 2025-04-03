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

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        anim.SetBool("shootingStraight", false);
        anim.SetBool("shootingDiagonal", false);
        boxCollider = GetComponent<BoxCollider2D>();
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
        body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
        anim.SetTrigger("jump");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
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
}