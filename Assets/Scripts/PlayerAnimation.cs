using System;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void SetRunning(bool isRunning)
    {
        anim.SetBool("running", isRunning);
    }

    public void SetGrounded(bool isGrounded)
    {
        anim.SetBool("grounded", isGrounded);
    }

    public void PlayJump()
    {
        Debug.Log(">>> Jump Triggered");
        anim.ResetTrigger("jump");
        anim.SetTrigger("jump");
    }

    public void UpdateShootingAnimation(bool isShooting, bool isRunning)
    {
        anim.SetBool("shStraight", isShooting && !isRunning);
        anim.SetBool("shRunStraight", isShooting && isRunning);
    }
}
