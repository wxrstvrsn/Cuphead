using UnityEngine;
using System;

public class Entity : MonoBehaviour
{
    [SerializeField] protected float speed;
    [SerializeField] protected float jumpForce;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask wallLayer;

    private Rigidbody2D body;
    private BoxCollider2D boxCollider;

    protected virtual void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    protected void Move(float direction)
    {
        if (!IsTouchingWall())
            body.linearVelocity = new Vector2(direction * speed, body.linearVelocity.y);
        Flip(direction);
    }

    protected void Jump()
    {
        if (IsGrounded())
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
        }
    }

    protected bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f,
            Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    protected bool IsTouchingWall()
    {
        return Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f,
            Vector2.right * transform.localScale.x, 0.3f, wallLayer);
    }

    protected void Flip(float direction)
    {
        if (direction > 0.01f)
            transform.localScale = Vector3.one;
        else if (direction < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}