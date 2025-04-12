using TMPro.EditorUtilities;
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
    public void UpdateShootingAnimation(bool isShooting, bool isRunning)
    {
        _anim.SetBool("shStraight", isShooting && !isRunning);
    }

    /// <summary>
    /// Активация анимации реакции персонажа на получение урона
    /// </summary>
    public virtual void PlayHit()
    {
        _anim.SetTrigger("hit");
    }

    public void SetDashing(bool isDashing)
    {
        _anim.SetBool("dashing", isDashing);
    }
}