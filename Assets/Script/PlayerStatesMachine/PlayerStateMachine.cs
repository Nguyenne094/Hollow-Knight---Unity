using System;
using UnityEngine;
using PlayerStates;

public class PlayerStateMachine : MonoBehaviour
{
    public IState CurrentState { get; private set; }
    public Player Player;
    
    public Idle Idle;
    public Walk Walk;
    public Attack Attack;

    public event Action OnStateChanged;
    
    public PlayerStateMachine(Player player)
    {
        Player = player;
        Idle = new Idle(this);
        Walk = new Walk(this);
        Attack = new Attack(this);
    }

    public void Initialize(IState  startingState)
    {
        CurrentState = startingState;
        startingState.Enter();
    }

    public void TransitionTo(IState nextState)
    {
        CurrentState.Exit();
        CurrentState = nextState;
        CurrentState.Enter();
        
        OnStateChanged?.Invoke();
    }

    public void Tick()
    {
        if (CurrentState != null)
        {
            CurrentState.Update();
        }
    }
}