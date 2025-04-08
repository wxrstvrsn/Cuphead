using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform bulletHomePoint;
    [SerializeField] private GameObject[] bullets;

    private Player _player;
    private PlayerAnimation playerAnimation;
    private float coolDownTimer = Mathf.Infinity;

    private bool isShooting;

    private void Awake()
    {
        _player = GetComponent<Player>();
        playerAnimation = GetComponent<PlayerAnimation>();
    }

    private void Update()
    {
        isShooting = Input.GetKey(KeyCode.Z);
        UpdateAnimation();

        HandleShooting();
    }

    private void HandleShooting()
    {
        if (isShooting && coolDownTimer > attackCooldown)
        {
            Debug.Log("HANDLE SHOOTING");
            coolDownTimer = 0;
            bullets[FindActiveBullet()].transform.position = bulletHomePoint.position;
            bullets[FindActiveBullet()].GetComponent<Bullet>().SetDirection(Mathf.Sign(_player.transform.localScale.x));
        }
    }

    private void UpdateAnimation()
    {
        playerAnimation.UpdateShootingAnimation(isShooting, _player.IsRunning());
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