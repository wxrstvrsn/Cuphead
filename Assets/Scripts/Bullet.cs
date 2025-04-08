using System;
using UnityEngine;

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

        float movementSpeed = _direction * speed * Time.deltaTime;
        transform.Translate(Vector3.right * movementSpeed);

        // some fx -- continuous shaking along oY axis
        float wavePower = Mathf.Sin(Time.time * 10f) * 0.01f;
        transform.position += new Vector3(0f, wavePower, 0f);
        
        /*TODO: исправить баг с тем, что при начале движения во
            время стрельбы пули друг друга догоняют
            может, необходимо добавить некую зависимость
            от положения игрока (хотя и так есть BulletPoint)
            короч подумать надо*/

        _lifeTime += Time.deltaTime;
        if (_lifeTime > bulletLifeTime)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _hit = true;
        _boxCollider.enabled = false;
        _anim.SetTrigger("explode");
    }

    public void SetDirection(float _direction)
    {
        Debug.Log("SET DIRECTION");
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