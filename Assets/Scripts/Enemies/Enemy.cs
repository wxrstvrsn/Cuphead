using UnityEngine;

/// <summary>
/// Базовый класс для всех противников
/// </summary>
public abstract class Enemy : Entity
{
    /// <summary>
    /// Расстояние до игрока для триггеринга активации противника
    /// </summary>
    [SerializeField] protected float _activationRadius;
    
    protected bool _isActive;

    protected EnemyAnimation _enemyAnimation;
    

    protected override void Awake()
    {
        base.Awake();
    }

    /// <summary>
    /// Геттер (испол-ся в PollManager) расстояния для активации противника 
    /// </summary>
    /// <returns></returns>
    public float GetActivationRadius() => _activationRadius;

    public virtual void Activate()
    {
        gameObject.SetActive(true);
        _isActive = true;
    }

    public virtual void Deactivate()
    {
        gameObject.SetActive(false);
        _isActive = false;
    }

    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        // костыль со StackOverflow
        var mono = other.collider.GetComponent<MonoBehaviour>();
        if (mono is IDamageable damageable)
        {
            damageable.GetDamage();
        }
    }
}