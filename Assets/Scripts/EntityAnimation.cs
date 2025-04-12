using System;
using UnityEngine;

public class EntityAnimation : MonoBehaviour
{
    protected Animator _anim;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    public virtual void SetRunning(bool isRunning)
    {
        _anim.SetBool("running", isRunning);
    }
    
    public virtual void SetGrounded(bool isGrounded)
    {
        _anim.SetBool("grounded", isGrounded);
    }

    public virtual void PlayJump()
    {
        // Debug.Log(">>> Jump Triggered");
        _anim.ResetTrigger("jump");
        _anim.SetTrigger("jump");
    }

    public virtual void PlayDeath()
    {
        _anim.SetTrigger("death");
    }
}
