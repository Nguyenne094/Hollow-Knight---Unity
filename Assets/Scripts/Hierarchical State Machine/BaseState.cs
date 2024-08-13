using System;
using UnityEditor;
using UnityEngine;

namespace Bap.State_Machine
{
    public abstract class BaseState
    {
        protected bool _isRoot = false;
        protected HierarchicalStateMachine _ctx;
        protected StateFactory _factory;
        protected BaseState _currentSuperState;
        protected BaseState _currentSubState;
        
        public bool IsRoot => _isRoot;

        public BaseState(HierarchicalStateMachine ctx, StateFactory factory)
        {
            _ctx = ctx;
            _factory = factory;
        }

        public abstract void Enter();
        public abstract void UpdateState();
        public abstract void Exit();
        protected abstract void CheckSwitchState();

        /// <summary>
        /// Switch to difference state within the same branch
        /// </summary>
        /// <param name="newState">State want to switch to</param>
        protected void SwitchState(BaseState newState)
        {
            Exit();
            newState.Enter();
            if (_isRoot)
            {
                _ctx.SetCurrentState(newState);
            }
            else if (_currentSuperState != null)
            {
                _currentSuperState.SetSubState(newState);
                _ctx.SetCurrentSubState(newState);
            }
            if (_ctx.AutoPause && _ctx.CurrentState._isRoot)
            {
                EditorApplication.isPaused = true;
            }
        }

        /// <summary>
        /// Set sub state for the current state
        /// </summary>
        /// <param name="subState">SubState want to switch to</param>
        protected void SetSubState(BaseState subState)
        {
            _currentSubState = subState;
            _currentSubState.SetSuperState(this);
        }

        protected void SetSuperState(BaseState superState)
        {
            _currentSuperState = superState;
        }

        /// <summary>
        /// Determine the sub state for the current state
        /// </summary>
        public abstract void InitializeSubState();

        public void UpdateStates()
        {
            UpdateState();
            _currentSubState?.UpdateStates();
        }
    }
}