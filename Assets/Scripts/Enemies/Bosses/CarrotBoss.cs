using System.Collections;
using UnityEngine;

public class CarrotBoss : Boss
{
    [Header("CarrotBoss Settings")] [SerializeField]
    private float introDuration = 2f;

    [SerializeField] private float laserDuration = 3f;
    [SerializeField] private float carrotLaunchCooldown = 1f;

    [Header("Attack References")] [SerializeField]
    private Transform[] firePoints;

    [SerializeField] private CarrotBulletPool bulletPool;
    [SerializeField] private Transform target;
    [SerializeField] private float carrotBulletSpeed = 5f;

    private CarrotAnimation _carrotAnimation;
    private CarrotBossState _currentState;

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
    }

    private void Start()
    {
        StartCoroutine(StateMachine());
    }

    private IEnumerator StateMachine()
    {
        yield return DoIntro();

        while (_currentState != CarrotBossState.Death)
        {
            CarrotBossState nextState =
                Random.Range(0, 2) == 0 ? CarrotBossState.LaunchCarrots : CarrotBossState.LaserShoot;

            switch (nextState)
            {
                case CarrotBossState.LaunchCarrots:
                    _carrotAnimation.PlayLaunchCarrots();
                    yield return LaunchCarrots();
                    break;
                case CarrotBossState.LaserShoot:
                    _carrotAnimation.PlayLaser();
                    yield return LaserShoot();
                    break;
            }

            yield return new WaitForSeconds(2f);
        }

        yield return DeathSequence();
    }

    private IEnumerator DoIntro()
    {
        _currentState = CarrotBossState.Intro;
        _carrotAnimation.PlayIntro();
        yield return new WaitForSeconds(introDuration);
    }

    private IEnumerator LaunchCarrots()
    {
        _currentState = CarrotBossState.LaunchCarrots;

        for (int i = 0; i < 5; i++)
        {
            var carrot = bulletPool.GetCarrot();
            Transform spawnPoint = firePoints[Random.Range(0, firePoints.Length)];

            carrot.transform.position = spawnPoint.position;
            carrot.Activate(target, carrotBulletSpeed);
        }

        yield return new WaitForSeconds(carrotLaunchCooldown);
    }

    private IEnumerator LaserShoot()
    {
        _currentState = CarrotBossState.LaserShoot;
        yield return new WaitForSeconds(laserDuration);
    }

    public void Die()
    {
        if (_currentState != CarrotBossState.Death)
        {
            StopAllCoroutines();
            _currentState = CarrotBossState.Death;
            StartCoroutine(DeathSequence());
        }
    }

    private IEnumerator DeathSequence()
    {
        _carrotAnimation.PlayDeath();
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }
}