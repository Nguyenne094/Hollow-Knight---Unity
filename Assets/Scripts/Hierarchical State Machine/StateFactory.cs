using System.Collections.Generic;
using UnityEngine;

namespace Bap.State_Machine
{
    /// <summary>
    /// Caching all states as a factory
    /// </summary>
    public class StateFactory
    {
        public enum State{Ground, Jump, Idle, Walk, Attack, Fall, Slash, SlashUpward, SlashDownward, SlashForward, Hurt, Death, Dash }

        private HierarchicalStateMachine _ctx;
        private IDictionary<State, BaseState> m_states;
        
        public IDictionary<State, BaseState> States
        {
            get => m_states;
            private set => m_states = value;
        }
        
        public StateFactory(HierarchicalStateMachine ctx)
        {
            this._ctx = ctx;
            States = new Dictionary<State, BaseState>()
            {
                {State.Ground, new Ground(_ctx, this)},
                {State.Jump, new Jump(_ctx, this)},
                {State.Fall, new Fall(_ctx, this)},
                {State.Idle, new Idle(_ctx, this)},
                {State.Walk, new Walk(_ctx, this)},
                {State.Attack, new Attack(_ctx, this)}
            };
        }
        
        public BaseState GetGroundState()
        {
            return States[State.Ground];
        }
        
        public BaseState GetJumpState()
        {
            return States[State.Jump];
        }
        
        public BaseState GetIdleState()
        {
            return States[State.Idle];
        }
        
        public BaseState GetWalkState()
        {
            return States[State.Walk];
        }
        
        public BaseState GetAttackState()
        {
            return States[State.Attack];
        }
        
        public BaseState GetFallState()
        {
            return States[State.Fall];
        }
    }
}