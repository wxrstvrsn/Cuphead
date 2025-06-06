using UnityEngine;

/// <summary>
/// Базовый класс для всех противников
/// </summary>
public abstract class Enemy : Entity
{
    /// <summary>
    /// Расстояние до игрока для триггеринга активации противника
    /// </summary>
    [SerializeField] public float _activationRadius;

    [SerializeField] protected float _damageCooldown = 1f;
    protected float _damageCooldownTimer;

    protected /*override -- туда же -- апдейта нет*/ void Update()
    {
        // base.Update(); -- а у меня бля нет апдейта в базовом Entity...
        _damageCooldownTimer += Time.deltaTime;
    }


    protected override void Awake()
    {
        base.Awake();
    }

    /// <summary>
    /// Геттер (испол-ся в PollManager) расстояния для активации противника 
    /// </summary>
    /// <returns></returns>
    public float GetActivationRadius() => _activationRadius;

    public abstract void Activate();
    public abstract void Deactivate();

    
}