using Unity.Mathematics.Geometry;
using UnityEngine;
using Math = System.Math;

public class Entity : MonoBehaviour
{
    [SerializeField] protected float speed;
    [SerializeField] protected float jumpForce;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected LayerMask wallLayer;
    [SerializeField] int healthPoints;

    public Rigidbody2D _body { get; private set; }
    private CapsuleCollider2D _capsuleCollider;
    
    protected CapsuleCollider2D GetCapsuleCollider => _capsuleCollider;

    protected virtual void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    /// <summary>
    /// Обработчик движения
    /// </summary>
    /// <param name="direction"></param>
    protected void Move(float direction)
    {
        // Если уперлись в стену, то стоим не рыпаемся
        if (!IsTouchingWall())
        {
            // в противном случае шевелилку подрубаем
            _body.linearVelocity = new Vector2(direction * speed, _body.linearVelocity.y);
        }
        Flip(direction);
    }

    protected void Jump()
    {
        // Если на чем-т стоим
        if (IsGrounded() || IsGroundedOnWall())
        {
            // прыгалка
            _body.linearVelocity = new Vector2(_body.linearVelocity.x, jumpForce);
            // print($"jupForce in Entity :: Jump: {jumpForce}");
        }
    }

    protected bool IsGrounded()
    {
        // имеем горизонтальный луч размером bounds.size,
        // пускаем рэйкаст лучи из каждой вещественной точки
        // луча (см выше) вниз на длину 0.1f в поисках земли (groundLayer)
        RaycastHit2D raycastHitGround = Physics2D.BoxCast(_capsuleCollider.bounds.center, _capsuleCollider.bounds.size,
            0f,
            Vector2.down, 0.1f, groundLayer);
        return raycastHitGround;
    }

    protected bool IsGroundedOnWall()
    {
        // аналогично IsGrounde(), но чекаем коллизию со стеной
        // + семейство лучей пуляем чуть с другой позиции
        Vector2 center = _capsuleCollider.bounds.center;
        
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

        // пуляем семейство рэйкаст лучей в сторону, куда пялит перс, в поисках стены
        return Physics2D.BoxCast(center, size, 0f, Vector2.right * direction, 0.1f, wallLayer);
    }

    /// <summary>
    /// Зеркалим по горизонтали в зависимости от направления движения
    /// </summary>
    /// <param name="direction"></param>
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