using UnityEngine;

public class CarrotAnimation : EntityAnimation
{
    public void PlayIntro()
    {
        _anim.SetTrigger("intro");
    }

    public void PlayLaser()
    {
        _anim.SetTrigger("laser");
    }

    public void PlayLaunchCarrots() => _anim.SetTrigger("carrots");

    public void PlayPrepare() => _anim.SetTrigger("prepare");
}
