using System;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    
    private Player _player;
    private PlayerAnimation playerAnimation;

    private bool isShooting;

    private void Awake()
    {
        _player = GetComponent<Player>();
        playerAnimation = GetComponent<PlayerAnimation>();
    }

    private void Update()
    {
        HandleShooting();
    }

    private void HandleShooting()
    {
        isShooting = Input.GetKey(KeyCode.Z);
        playerAnimation.UpdateShootingAnimation(isShooting, _player.IsRunning());
    }
}