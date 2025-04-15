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
}