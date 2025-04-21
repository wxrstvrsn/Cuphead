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

    private float _direction;
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
        float movementSpeed = (_direction == 0) ? speed * Time.deltaTime : _direction * speed * Time.deltaTime;
        float wavePower = Mathf.Sin(Time.time * Mathf.PI * _direction);

        if (_direction == 0)
        {
            transform.Rotate(-90,0,0);
            transform.Translate(Vector3.up * movementSpeed);
            transform.position += new Vector3(wavePower, 0f, 0f);
            
        }
        else
        {
            transform.Translate(Vector3.up * movementSpeed);

            // some fx -- continuous shaking along oY axis
            transform.position += new Vector3(0f, wavePower, 0f);
        }

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
            damageable.GetDamage();
        }

        _hit = true;
        _boxCollider.enabled = false;
        _anim.SetTrigger("explode");
        //TODO: вместо двух строк последних -- просто SetTrigger("explode") + add event в Animation
        // по завершении анимации "explode" Дергаем Deactivate(); 
    }

    public void SetDirection(float _direction)
    {
        /*Debug.Log("SET DIRECTION");*/
        _lifeTime = 0;
        this._direction = _direction;
        gameObject.SetActive(true);
        _hit = false;
        _boxCollider.enabled = true;

        float localScaleX = transform.localScale.x;
        if (Math.Sign(localScaleX) != _direction)
            localScaleX = -localScaleX;
        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}