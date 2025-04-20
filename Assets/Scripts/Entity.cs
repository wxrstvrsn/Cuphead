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
        Vector2 capsuleCenter = _capsuleCollider.bounds.center;
        float radius = _capsuleCollider.size.x * Mathf.Abs(transform.localScale.x);
        float offsetY = (_capsuleCollider.bounds.size.y / 2f) - radius;
        float castDistance = 0.05f;

        Vector2 circleCastOrigin = capsuleCenter + Vector2.down * offsetY;

        RaycastHit2D hit = Physics2D.CircleCast(circleCastOrigin, radius, Vector2.down, castDistance, groundLayer);

        return hit.collider;
    }


    //FIXED: BoxCast -->> CircleCast
    //  fix rayCast length 
    //  если игроком бежим по диагонали вверх и прыгаем, 
    //  то бывает анимация прыжка сбивается из-за того, ч цепляем справа выше землю коллизией 
    // либо как т через Animation / Animator 
    // запретить сменять анимацию прыжка на следующую, пока эта не отыграла
    protected bool IsGroundedOnWall()
    {
        // аналогично IsGrounded(), но чекаем коллизию со стеной
        // + семейство лучей пуляем чуть с другой позиции

        {
            Vector2 capsuleCenter = _capsuleCollider.bounds.center;
            float radius = _capsuleCollider.size.x * Mathf.Abs(transform.localScale.x);
            float offsetY = (_capsuleCollider.bounds.size.y / 2f) - radius;
            float castDistance = 0.05f;

            Vector2 circleCastOrigin = capsuleCenter + Vector2.down * offsetY;

            RaycastHit2D hit = Physics2D.CircleCast(circleCastOrigin, radius, Vector2.down, castDistance, wallLayer);

            return hit.collider;
        }
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

    private void Die()
    {
        gameObject.SetActive(false);
    }
}