using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] protected float speed;
    [SerializeField] protected float jumpForce;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask wallLayer;

    private Rigidbody2D _body;
    private BoxCollider2D _boxCollider;

    protected virtual void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    protected void Move(float direction)
    {
        if (!IsTouchingWall())
            _body.linearVelocity = new Vector2(direction * speed, _body.linearVelocity.y);
        Flip(direction);
    }

    protected void Jump()
    {
        if (IsGrounded())
        {
            _body.linearVelocity = new Vector2(_body.linearVelocity.x, jumpForce);
        }
    }

    protected bool IsGrounded()
    {
        RaycastHit2D raycastHitGround = Physics2D.BoxCast(_boxCollider.bounds.center, _boxCollider.bounds.size, 0f,
            Vector2.down, 0.1f, groundLayer);
        RaycastHit2D raycastHitWall = Physics2D.BoxCast(_boxCollider.bounds.center, _boxCollider.bounds.size, 0f,
            Vector2.down, 0.1f, wallLayer);
        return raycastHitWall || raycastHitGround;
    }

    protected bool IsTouchingWall()
    {
        return Physics2D.BoxCast(_boxCollider.bounds.center, _boxCollider.bounds.size, 0f,
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
}