using UnityEngine;
using UnityEngine.PlayerLoop;

public class ShootingEnemy : Enemy
{
    [SerializeField] private GameObject[] bullets;
    [SerializeField] private Transform bulletHomePoint;
    [SerializeField] private float attackCooldown;

    private float _cooldownTimer = Mathf.Infinity;
    private bool _isShooting;

    private Player _player;
    private Vector2 _bulletDirection;
    private EnemyAnimation _enemyAnimation;

    private void Update()
    {
        UpdateAnimation();
        HandleShooting();
    }

    private void HandleShooting()
    {
        if (_isShooting && _cooldownTimer > attackCooldown)
        {
            _cooldownTimer = 0;
            int index = FindActiveBullet();
            bullets[index].transform.position = bulletHomePoint.position;
            // bullets[index].GetComponent<Bullet>().transform.position.; = ;
        }
    }

    private void UpdateAnimation()
    {
    }


    public override void Activate()
    {
    }

    public override void Deactivate()
    {
    }
    
    private int FindActiveBullet()
    {
        for (int i = 0; i < bullets.Length; i++)
        {
            if (!bullets[i].activeInHierarchy)
                return i;
        }

        return 0;
    }
}