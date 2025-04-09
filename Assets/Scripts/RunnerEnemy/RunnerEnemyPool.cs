using System;
using Unity.VisualScripting;
using UnityEngine;

// TODO: реализовать корректную активацию
//  дергать активацию из класса object pooler'a

public class RunnerEnemyPool : MonoBehaviour
{
    [SerializeField] private RunningEnemy[] _runners;
    [SerializeField] private float _activationRadius;
    [SerializeField] private Transform _target;

    private float[] _timers;

    private void Awake()
    {
        _timers = new float[_runners.Length];
        for (int i = 0; i < _runners.Length; i++)
        {
            _runners[i].gameObject.SetActive(false);
            _timers[i] = 0;
        }
    }

    private void Update()
    {
        for (int i = 0; i < _runners.Length; i++)
        {
            _timers[i] += Time.deltaTime;

            if ((_timers[i] >= _runners[i].GetTimeToLive())
                && (_runners[i].gameObject.activeInHierarchy)
                && (Vector2.Distance(_target.position, _runners[i].transform.position) <= _activationRadius))
            {
            }
        }
    }
}