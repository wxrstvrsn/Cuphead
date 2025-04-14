/*using System;
using UnityEngine;

public class GroundDustFX : MonoBehaviour
{
    [SerializeField] private GameObject landingDustPrefab;
    // [SerializeField] private Transform dustSpawnPoint;
    private Player _player;

    private bool wasGroundedLastFrame;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void Update()
    {
        bool isGrounded = _player.isGrounded();

        if (isGrounded && !wasGroundedLastFrame)
        {
            Instantiate(landingDustPrefab, this.transform.position, Quaternion.identity);
        }

        wasGroundedLastFrame = isGrounded;
    }
}*/