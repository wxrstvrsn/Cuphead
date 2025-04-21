using UnityEngine;

public class CarrotAnimation : EntityAnimation
{
    private void PlayIntro() => _anim.SetTrigger("intro");
    
    private void PlayLaser() => _anim.SetTrigger("laser");
    
    private void PlayDeath() => _anim.SetTrigger("death");
    
    private void PlayLaunchCarrots() => _anim.SetTrigger("carrots");
    
    private void PlayPrepare() => _anim.SetTrigger("prepare");
}
