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
        Vector3 waveFx = Vector3.zero;

        // some fx -- continuous shaking 
        float wavePower = Mathf.Sin(Time.time * Mathf.PI * 2f) * 0.01f;
        if (_direction.y == 0)
        {
            waveFx = new Vector3(0f, wavePower, 0f);
        }
        else if (_direction.x == 0)
        {
            waveFx = new Vector3(wavePower, 0f, 0f);
        }

        transform.position += waveFx;
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

        Debug.Log($"Bullet launched! POS: {transform.position} | DIR: {_direction}");
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