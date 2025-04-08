using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator _anim;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    public void SetRunning(bool isRunning)
    {
        _anim.SetBool("running", isRunning);
    }

    public void SetGrounded(bool isGrounded)
    {
        _anim.SetBool("grounded", isGrounded);
    }

    public void PlayJump()
    {
        Debug.Log(">>> Jump Triggered");
        _anim.ResetTrigger("jump");
        _anim.SetTrigger("jump");
    }

    public void UpdateShootingAnimation(bool isShooting, bool isRunning)
    {
        _anim.SetBool("shStraight", isShooting && !isRunning);
    }
}
