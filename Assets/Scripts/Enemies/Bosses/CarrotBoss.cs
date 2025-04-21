using System;
using UnityEngine;
using System.Collections;

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
            yield return StartCoroutine(LaunchCarrots());
            yield return StartCoroutine(LaserShoot());
        }
    }

    private IEnumerator DoIntro()
    {
        _currentState = CarrotBossState.Intro;
        Debug.Log("Carrot Boss Intro");
        _enemyAnimation.PlayIntro();
        
        yield return new WaitForSeconds(_introDuration);
        
        Debug.Log("Carrot Boss Intro FINISHED");
    }
}
