using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class CarrotBoss : Boss, IDamageable
{
    [Header("Attacking")]
    [SerializeField] private Transform[] firePoints;
    [SerializeField] private Transform playerTarget;
    [SerializeField] private CarrotBulletPool bulletPool;
    [SerializeField] private LaserPool laserPool;

    [Header("Carrot Settings")]
    [SerializeField] private int baseCarrotBulletCount;
    [SerializeField] private float baseCarrotBulletCooldown;

    [Header("Laser Settings")]
    [SerializeField] private int laserBaseCount;
    [SerializeField] private float laserInterval;

    [Header("Behaviour")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxAggressionMultiplicator;
    [SerializeField] private float baseIdleDuration;

    private CarrotAnimation _carrotAnimation;

    private enum CarrotBossState
    {
        Intro,
        LaunchCarrots,
        LaserShoot,
        Death
    }

    private CarrotBossState _currentState;

    protected override void Awake()
    {
        base.Awake();
        _carrotAnimation = GetComponent<CarrotAnimation>();
    }

    private void Start()
    {
        StartCoroutine(StateMachine());
    }

    private float AggressionMultiplier()
    {
        float delta = maxHealth - currentHealth;
        return 1f + (delta / maxHealth) * (maxAggressionMultiplicator - 1f);
    }

    private IEnumerator StateMachine()
    {
        yield return StartCoroutine(PlayIntro());

        while (_currentState != CarrotBossState.Death)
        {
            if (currentHealth <= 0f)
            {
                _currentState = CarrotBossState.Death;
                break;
            }

            float aggression = AggressionMultiplier();

            CarrotBossState nextState = (Random.Range(0, 2) == 0)
                ? CarrotBossState.LaunchCarrots
                : CarrotBossState.LaserShoot;

            yield return StartCoroutine(PlayPrepare());

            switch (nextState)
            {
                case CarrotBossState.LaunchCarrots:
                    yield return StartCoroutine(LaunchCarrots(aggression));
                    break;
                case CarrotBossState.LaserShoot:
                    yield return StartCoroutine(LaserAttack(aggression));
                    break;
            }

            yield return StartCoroutine(PlayIdle(baseIdleDuration / aggression));
            
        }

        yield return StartCoroutine(DeathSequence());
    }

    private IEnumerator PlayIntro()
    {
        _currentState = CarrotBossState.Intro;
        _carrotAnimation.PlayIntro();
        yield return new WaitForSeconds(2.5f);
    }

    private IEnumerator PlayPrepare()
    {
        _carrotAnimation.PlayPrepare();
        yield return new WaitForSeconds(1f); // длительность анимации подготовки
                                             // захардкоженная ибо в аниматоре 1сек длительность анимки
    }

    private IEnumerator PlayIdle(float baseIdleDuration)
    {
        _carrotAnimation.PlayIdle();
        yield return new WaitForSeconds(baseIdleDuration); // время, пока он в состоянии ожидания
    }

    private IEnumerator LaunchCarrots(float aggression)
    {
        _currentState = CarrotBossState.LaunchCarrots;

        int count = Mathf.RoundToInt(baseCarrotBulletCount * aggression);
        float delay = baseCarrotBulletCooldown / aggression;

        _carrotAnimation.PlayLaunchCarrots();

        for (int i = 0; i < count; i++)
        {
            var bullet = bulletPool.GetBullet();
            bullet.transform.position = firePoints[Random.Range(0, firePoints.Length)].position;
            bullet.Launch(playerTarget);
            yield return new WaitForSeconds(delay);
        }
    }

    private IEnumerator LaserAttack(float aggression)
    {
        _currentState = CarrotBossState.LaserShoot;

        int count = Mathf.RoundToInt(laserBaseCount * aggression);
        float delay = laserInterval / aggression;

        _carrotAnimation.PlayLaser();

        Vector2 direction = (playerTarget.position - transform.position).normalized;

        for (int i = 0; i < count; i++)
        {
            var laser = laserPool.GetLaserSegment();
            laser.transform.position = transform.position;
            laser.Launch(direction);
            yield return new WaitForSeconds(delay);
        }
    }

    private IEnumerator DeathSequence()
    {
        _currentState = CarrotBossState.Death;
        _carrotAnimation.PlayDeath();
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }

    public void GetDamage()
    {
        currentHealth -= 1f;
        if (currentHealth <= 0f && _currentState != CarrotBossState.Death)
        {
            StopAllCoroutines();
            StartCoroutine(DeathSequence());
        }
    }
}
