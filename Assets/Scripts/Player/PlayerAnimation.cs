using UnityEngine;

public class PlayerAnimation : EntityAnimation
{
    /// <summary>
    /// Активация анимации Рывка
    /// </summary>
    public void PlayDash()
    {
        _anim.SetTrigger("dash");
    }

    /// <summary>
    /// Апдейтер значения булевой переменной аниматора "shStraight" 
    /// </summary>
    /// <param name="isShooting"> Булевая переменная аниматора для трекинга состояния стрельбы </param>
    /// <param name="isRunning"> Булевая переменная аниматора для трекинга состояния бега </param>
    public void UpdateShootingAnimation(bool isShooting, bool isRunning, bool lookUp)
    {
        _anim.SetBool("shStraight", isShooting && !isRunning);
        _anim.SetBool("shootUp", isShooting && lookUp);
    }

    /// <summary>
    /// Активация анимации реакции персонажа на получение урона
    /// </summary>
    public virtual void PlayHit()
    {
        _anim.ResetTrigger("hit");
        _anim.SetTrigger("hit");
    }

    /// <summary>
    /// Апдейтер значения булевой переменной аниматора "dashing" 
    /// </summary>
    /// <param name="isDashing"></param>
    public void SetDashing(bool isDashing)
    {
        _anim.SetBool("dashing", isDashing);
    }
}