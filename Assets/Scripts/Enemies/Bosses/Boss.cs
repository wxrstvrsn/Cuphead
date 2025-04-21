using System;
using UnityEngine;
using System.Collections;

public abstract class Boss : Enemy, IDamageable
{
    [Header("Boss Settings")] [SerializeField]
    protected int maxHealth;

    
    protected virtual void Awake()
    {
        //
    }
    
    public void GetDamage()
    {
    }
}