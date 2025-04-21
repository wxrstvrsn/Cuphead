using UnityEngine;

public class EnemyAnimation : EntityAnimation
{
    public void PlayHide()
    {
        _anim.SetTrigger("hide");
    }

    public void PlayPopOut()
    {
        _anim.SetTrigger("popOut");
    }

    public bool GetRunning() => _anim.GetBool("running");

    public void PlayIdle()
    {
        _anim.SetTrigger("idle");
    }

    public void PlayIntro()
    {
        _anim.SetTrigger("intro");
    }

    public void PlayLaserShoot()
    {
        // TODO
        throw new System.NotImplementedException();
    }

    public void PlayLaunchCarrots()
    {
        throw new System.NotImplementedException();
    }
}