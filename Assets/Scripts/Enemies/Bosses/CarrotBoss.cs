using UnityEngine;
using System.Collections;

public class CarrotBoss : MonoBehaviour
{
    [SerializeField] private Transform[] firePoints;
    [SerializeField] private CarrotBulletPool bulletPool;
    [SerializeField] private Transform target;

    private void Start()
    {
        StartCoroutine(FireCarrotsRoutine());
    }

    private IEnumerator FireCarrotsRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);

            var bullet = bulletPool.GetBullet();
            bullet.transform.position = firePoints[Random.Range(0, firePoints.Length)].position;
            bullet.Launch(target);
        }
    }
}