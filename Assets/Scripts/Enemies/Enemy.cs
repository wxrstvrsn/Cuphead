using UnityEngine;

/// <summary>
/// Базовый класс для всех противников
/// </summary>
public abstract class Enemy : Entity
{
    public abstract void Activate();
    public abstract void Deactivate();
}