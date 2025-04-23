using System.Collections;
using UnityEngine;

public class CarrotBoss : Boss
{
    [Header("Carrot Settings")]
    [SerializeField] private CarrotBulletPool bulletPool;
    [SerializeField] private Transform[] firePoints;
    [SerializeField] private Transform playerTarget;

    [Header("Laser Settings")]
    [SerializeField] private LaserPool laserPool;
    [SerializeField] private int laserCount = 10;
    [SerializeField] private float laserInterval = 0.2f;

    [Header("State Settings")]
    [SerializeField] private float carrotCooldown = 3f;
    [SerializeField] private float laserCooldown = 4f;
    [SerializeField] private float maxIdleTime = 6f;

    private CarrotAnimation _carrotAnimation;
    private CarrotBossState _state = CarrotBossState.Intro;
    private float _lastHealthCheck;
    private int _lastHealth;

    private enum CarrotBossState
    {
        Intro,
        LaunchCarrots,
        LaserShoot,
        Death
    }

    protected override void Awake()
    {
        base.Awake();
        _carrotAnimation = GetComponent<CarrotAnimation>();
        _lastHealth = _healthPoints;
        _lastHealthCheck = Time.time;
    }

    private void Start()
    {
        StartCoroutine(BossRoutine());
    }

    private IEnumerator BossRoutine()
    {
        yield return StartCoroutine(PlayIntro());

        while (_state != CarrotBossState.Death)
        {
            if (_healthPoints <= 0)
            {
                _state = CarrotBossState.Death;
                break;
            }

            bool hasTakenDamage = _healthPoints < _lastHealth;

            if (hasTakenDamage)
            {
                _lastHealth = _healthPoints;
                _lastHealthCheck = Time.time;
            }

            if (!hasTakenDamage && Time.time - _lastHealthCheck >= maxIdleTime)
            {
                _state = (Random.value < 0.5f) ? CarrotBossState.LaunchCarrots : CarrotBossState.LaserShoot;
            }

            switch (_state)
            {
                case CarrotBossState.LaunchCarrots:
                    yield return StartCoroutine(FireCarrotsRoutine());
                    _state = CarrotBossState.LaserShoot;
                    break;

                case CarrotBossState.LaserShoot:
                    yield return StartCoroutine(FireLaserRoutine());
                    _state = CarrotBossState.LaunchCarrots;
                    break;
            }

            yield return new WaitForSeconds(1f);
        }

        yield return StartCoroutine(DeathSequence());
    }

    private IEnumerator PlayIntro()
    {
        _state = CarrotBossState.Intro;
        _carrotAnimation.PlayIntro();
        yield return new WaitForSeconds(2.5f); // длительность интро
        _state = CarrotBossState.LaunchCarrots;
    }

    private IEnumerator FireCarrotsRoutine()
    {
        _carrotAnimation.PlayLaunchCarrots();
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < 5; i++)
        {
            var bullet = bulletPool.GetBullet();
            bullet.transform.position = firePoints[Random.Range(0, firePoints.Length)].position;
            bullet.Launch(playerTarget);
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(carrotCooldown);
    }

    private IEnumerator FireLaserRoutine()
    {
        _carrotAnimation.PlayLaser();
        yield return new WaitForSeconds(0.5f);

        Vector2 direction = (playerTarget.position - transform.position).normalized;

        for (int i = 0; i < laserCount; i++)
        {
            var laser = laserPool.GetLaserSegment();
            laser.transform.position = transform.position;
            laser.Launch(direction);
            yield return new WaitForSeconds(laserInterval);
        }

        yield return new WaitForSeconds(laserCooldown);
    }

    private IEnumerator DeathSequence()
    {
        _carrotAnimation.PlayDeath();
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }

    public void ReceiveDamage()
    {
        _healthPoints--;
    }
}
