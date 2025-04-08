using UnityEngine;

public class PlayerAnimation : EntityAnimation
{
    public void PlayDash()
    {
        _anim.SetTrigger("dash");
    }
}