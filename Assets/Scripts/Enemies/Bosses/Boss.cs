using UnityEngine;
using System.Collections;

public abstract class Boss : Entity, IDamageable
{
    [Header("Boss Settings")]
    [SerializeField] protected int maxHealth;
    [SerializeField] protected float phaseTransitionDelay = 2f;

    

    
}