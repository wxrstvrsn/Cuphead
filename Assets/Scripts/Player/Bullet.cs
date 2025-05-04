using UnityEngine;
using UnityEngine.EventSystems;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private Transform spriteTransform;
    [SerializeField] private Animator anim;
    [SerializeField] private BoxCollider2D boxCollider;

    private Vector2 _direction;
    private float _timer;
    private bool _hit;

    private void Update()
    {
        if (_hit) return;

        Move();

        _timer += Time.deltaTime;
        if (_timer > lifetime)
            Deactivate();
    }

    private void Move()
    {
        transform.Translate(_direction.normalized * (speed * Time.deltaTime));
    }

    public void SetDirection(Vector2 direction)
    {
        _direction = direction.normalized;
        _timer = 0f;
        _hit = false;
        gameObject.SetActive(true);
        boxCollider.enabled = true;

        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        spriteTransform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //KOCTbIJLb ebony
        if(collision.CompareTag("Player")) return;
        
        if (collision.TryGetComponent<IDamageable>(out var dmg))
            dmg.GetDamage();

        _hit = true;
        boxCollider.enabled = false;
        Deactivate();
        //TODO: вместо двух строк последних -- просто SetTrigger("explode") + add event в Animation
        // по завершении анимации "explode" Дергаем Deactivate(); 
    }


    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}