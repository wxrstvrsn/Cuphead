using UnityEngine;
using System.Collections;

public class CarrotBoss : MonoBehaviour
{
    [Header("Carrot Bullet Settings")]
    [SerializeField] private Transform[] firePoints;
    [SerializeField] private CarrotBulletPool bulletPool;
    [SerializeField] private Transform target;

    [Header("Laser Attack Settings")]
    [SerializeField] private LaserPool laserPool;
    [SerializeField] private int laserCount = 10;
    [SerializeField] private float laserInterval = 0.2f;

    private void Start()
    {
        StartCoroutine(BossAttackRoutine());
    }

    private IEnumerator BossAttackRoutine()
    {
        while (true)
        {
            yield return StartCoroutine(FireCarrotsRoutine());
            yield return new WaitForSeconds(3f); // пауза между атаками
            yield return StartCoroutine(FireLaserRoutine());
            yield return new WaitForSeconds(3f); // пауза между атаками
        }
    }

    private IEnumerator FireCarrotsRoutine()
    {
        Debug.Log("Launching Carrots...");
        for (int i = 0; i < 5; i++)
        {
            var bullet = bulletPool.GetBullet();
            bullet.transform.position = firePoints[Random.Range(0, firePoints.Length)].position;
            bullet.Launch(target);
            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator FireLaserRoutine()
    {
        Debug.Log("Firing Laser Hypnosis!");
        Vector2 direction = (target.position - transform.position).normalized;

        for (int i = 0; i < laserCount; i++)
        {
            LaserSegment segment = laserPool.GetLaserSegment();
            segment.transform.position = transform.position;
            segment.Launch(direction);

            yield return new WaitForSeconds(laserInterval);
        }
    }
}