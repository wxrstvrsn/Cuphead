using System;
using UnityEngine;
using System.Collections;

public abstract class Boss : Entity, IDamageable
{
    [Header("Boss Settings")]
    [SerializeField] protected int maxHealth;
    [SerializeField] protected float phaseTransitionDelay = 2f;
    
    
    private Coroutine _stateCoroutine;

    // TODO:
    //      FSM - ???? мож попробовать??
    //      «Конечный  автомат — это простая структура, позволяющая определять сложное поведение» - о как
    protected enum State
    {
        Intro,
        Phase1,
        Phase2,
        Dead
    }
    protected State _state;

    protected virtual void Awake()
    {
        //
    }

    protected void EnterState(State state)
    {
        if(_stateCoroutine != null)
            StopCoroutine(_stateCoroutine);
        
        _state = state;

        switch (state)
        {
            case State.Intro:
                _stateCoroutine = StartCoroutine(PlayIntro());
        }
    }

    private IEnumerator PlayIntro()
    {
        throw new NotImplementedException();
    }


    public void GetDamage()
    {
        
    }
    
    
}