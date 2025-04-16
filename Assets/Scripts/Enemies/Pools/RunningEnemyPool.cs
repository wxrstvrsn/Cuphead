using UnityEngine;

// TODO: реализовать корректную активацию
//  дергать активацию из класса object pooler'a

public class RunnerEnemyPool : MonoBehaviour
{
    /// <summary>
    /// Ссылка на массив в Inspector для бегущих противников
    /// </summary>
    [SerializeField] private RunningEnemy[] _runners;

    /// <summary>
    /// Ссылка на игрока для регания расстояния до него от точки спавна противника
    /// </summary>
    [SerializeField] private Transform _target;

    /// <summary>
    /// Массив таймеров кулдауна респавна противников
    /// </summary>
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

    /*FIXED:
        Реализовать нормальную активацию объектов pool'a*/

    private void Update()
    {
        for (int i = 0; i < _runners.Length; i++)
        {
            _timers[i] += Time.deltaTime;
            /*FIXED
                а именно: ес проверка тру, тут дергаем Init() из RunningEnemy()
                который еще не написан(
                но над написать */
            bool allowedToSpawn = (!_runners[i].gameObject.activeInHierarchy &&
                                   Vector2.Distance(_target.position, _runners[i].transform.position) <
                                   _runners[i].GetActivationRadius() && _timers[i] >= _runners[i].GetSpawnCooldown());
            if (allowedToSpawn)
            {
                _runners[i].Activate();
                print("RUNNER ACTIVATED");
                _timers[i] = 0;
            }
        }
    }
}