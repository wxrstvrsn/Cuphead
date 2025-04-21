using System;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

public class CarrotBoss : Boss
{
    [Header("CarrotBoss Attacks")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float _carrotLaunchCooldown;
    [SerializeField] private float _laserDuration;
    [SerializeField] private float _introDuration;
    
    [SerializeField] private Transform[] firePoints;
    
    private CarrotBossState _currentState;

    private enum CarrotBossState
    {
        Intro,
        LaunchCarrots,
        LaserShoot,
        Death
    }

    private void Start()
    {
        StartCoroutine(StateMachine());
    }

    private IEnumerator StateMachine()
    {
        yield return StartCoroutine(DoIntro());

        while (_currentState != CarrotBossState.Death)
        {
            int rnd = Random.Range(0, 2);
            if (rnd == 0)
            {
                _currentState = CarrotBossState.LaserShoot;

                _enemyAnimation.PlayLaserShoot();
                yield return LaserShoot();
            }
            else
            {
                _currentState = CarrotBossState.LaunchCarrots;
                _enemyAnimation.PlayLaunchCarrots();
            }

            yield return new WaitForSeconds(10.0f);
        }
        
        _enemyAnimation.PlayDeath();
        yield return new WaitForSeconds(3.0f);
        gameObject.SetActive(false);
    }


    private IEnumerator DoIntro()
    {
        _currentState = CarrotBossState.Intro;
        Debug.Log("Carrot Boss Intro");
        _enemyAnimation.PlayIntro();
        
        yield return new WaitForSeconds(_introDuration);
        
        Debug.Log("Carrot Boss Intro FINISHED");
    }
    private IEnumerator LaunchCarrots()
    {
        _currentState = CarrotBossState.LaunchCarrots;
        Debug.Log("Carrot Boss Launch Carrots");
        
        // TODO: object pooling + CarrotBullet.cs
        
        yield return new WaitForSeconds(_carrotLaunchCooldown);
    }

    private IEnumerator LaserShoot()
    {
        _currentState = CarrotBossState.LaserShoot;
        Debug.Log("Carrot Boss Laser Shooting");
        
        yield return new WaitForSeconds(_laserDuration);
    }

    public void Die()
    {
        if (_currentState != CarrotBossState.Death)
        {
            StopAllCoroutines();
            _currentState = CarrotBossState.Death;
            Debug.Log("Carrot Boss Dead");
            // TODO: доделать анимацию + ...
        }
    }
    
    
}
