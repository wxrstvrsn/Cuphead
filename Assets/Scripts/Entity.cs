using Unity.Mathematics.Geometry;
using UnityEngine;
using Math = System.Math;

public class Entity : MonoBehaviour
{
    [SerializeField] protected float speed;
    [SerializeField] protected float jumpForce;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask wallLayer;
    [SerializeField] int healthPoints;

    public Rigidbody2D _body { get; private set; }
    private CapsuleCollider2D _capsuleCollider;

    protected virtual void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    protected void Move(float direction)
    {
        if (!IsTouchingWall())
        {
            _body.linearVelocity = new Vector2(direction * speed, _body.linearVelocity.y);
        }

        Flip(direction);
    }

    protected void Jump()
    {
        if (IsGrounded() || IsGroundedOnWall())
        {
            _body.linearVelocity = new Vector2(_body.linearVelocity.x, jumpForce);
            print($"jupForce in Entity :: Jump: {jumpForce}");
        }
    }

    protected bool IsGrounded()
    {
        RaycastHit2D raycastHitGround = Physics2D.BoxCast(_capsuleCollider.bounds.center, _capsuleCollider.bounds.size,
            0f,
            Vector2.down, 0.1f, groundLayer);
        return raycastHitGround;
    }

    protected bool IsGroundedOnWall()
    {
        Vector2 center = _capsuleCollider.bounds.center;

        // Размер кастомный — уже, но по высоте такой же (или меньше)
        Vector2 size = new Vector2(_capsuleCollider.bounds.size.x * 0.8f, _capsuleCollider.bounds.size.y);

        float distance = 0.1f;

        RaycastHit2D raycastHitWall = Physics2D.BoxCast(center, size, 0f, Vector2.down, distance, wallLayer);
        return raycastHitWall.collider != null;
    }

    protected bool IsTouchingWall()
    {
        Vector2 center = _capsuleCollider.bounds.center;
        float direction = Mathf.Sign(transform.localScale.x);
        center.x += direction * _capsuleCollider.bounds.extents.x * 0.9f; // смещаем ближе к краю

        Vector2 size = _capsuleCollider.bounds.size;
        size.y *= 0.5f; // Уменьшаем по высоте, чтобы не ловить стену в прыжке

        return Physics2D.BoxCast(center, size, 0f, Vector2.right * direction, 0.1f, wallLayer);
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

    protected void GetDamage()
    {
        healthPoints = Math.Max(healthPoints - 1, 0);

        if (healthPoints <= 0)
        {
            Die();
        }
        else
        {
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }
}