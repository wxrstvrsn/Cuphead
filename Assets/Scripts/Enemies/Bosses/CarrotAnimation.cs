using UnityEngine;

public class CarrotAnimation : EntityAnimation
{
    public void PlayIntro() => _anim.SetTrigger("intro");
    public void PlayPrepare() => _anim.SetTrigger("prepare");
    public void PlayIdle() => _anim.SetTrigger("idle");
    public void PlayLaunchCarrots() => _anim.SetTrigger("carrotShoot");
    public void PlayLaser() => _anim.SetTrigger("laserShoot");
    public override void PlayDeath() => _anim.SetTrigger("death");
}
