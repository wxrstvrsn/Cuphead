using System;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    
    private PlayerMovement playerMovement;
    private PlayerAnimation playerAnimation;

    private bool isShooting;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimation = GetComponent<PlayerAnimation>();
    }

    private void Update()
    {
        HandleShooting();
    }

    private void HandleShooting()
    {
        isShooting = Input.GetKey(KeyCode.Z);
        playerAnimation.UpdateShootingAnimation(isShooting, playerMovement.IsRunning());
    }
}