using System;
using UnityEngine;

/*
TODO:
    сделать абстрактным и наследовать PlayerBullet и EnemyBullet ??????
    RigidBody2D надо вешать на пульку - ???
    Если делать стрельбу типа по параболе -- gravity и сложная реализцаия
    расчета вектора, по которой пускать пульку из врага, чтобы
    она по баллистике попала в игрока, если он не увернется...
    трайхард кароче
    */

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float bulletLifeTime;
    [SerializeField] Transform spriteTransform;

    private Vector2 _direction;
    private bool _hit;
    private float _lifeTime;

    private Animator _anim;
    private BoxCollider2D _boxCollider;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (_hit) return;

        Move();

        _lifeTime += Time.deltaTime;
        if (_lifeTime > bulletLifeTime)
            Deactivate();
        // !!! деактивируем по времени жизни пульки -->> если 
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


        /*FIXED: исправить баг с тем, что при начале движения во
            время стрельбы пули друг друга догоняют
            может, необходимо добавить некую зависимость
            от положения игрока (хотя и так есть BulletPoint)
            короч подумать надо*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IDamageable>(out var damageable))
        {
            if (!collision.CompareTag("Player"))
                damageable.GetDamage();
        }

        _hit = true;
        _boxCollider.enabled = false;
        _anim.SetTrigger("explode");
        //TODO: вместо двух строк последних -- просто SetTrigger("explode") + add event в Animation
        // по завершении анимации "explode" Дергаем Deactivate(); 
    }

    public void SetDirection(Vector2 direction)
    {
        /*Debug.Log("SET DIRECTION");*/
        _lifeTime = 0;
        _direction = direction.normalized;

        gameObject.SetActive(true);

        _hit = false;
        _boxCollider.enabled = true;

        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        spriteTransform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}