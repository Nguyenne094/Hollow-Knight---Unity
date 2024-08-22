using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Bap.State_Machine
{
    /// <summary>
    /// Manage context logic and the transitions of all states
    /// </summary>
    public class HierarchicalStateMachine : MonoBehaviour
    {
        public string CurrentStateView;
        public string CurrentSubStateView;

        [SerializeField] private bool autoPause = true;
        [SerializeField] private float attackCooldown = 0.5f;
        
        private BaseState _currentState;
        private BaseState _currentSubState;
        private StateFactory _stateFactory;
        protected Player _player;
        
        [SerializeField] private List<string> m_stateList;

        #region Properties

        public List<string> StateList
        {
            get { return m_stateList; }
            set { m_stateList = value; }
        }
        public BaseState CurrentState
        {
            get => _currentState;
            private set
            {
                _currentState = value;
                CurrentStateView = _currentState.GetType().Name;
            }
        }
        public BaseState CurrentSubState
        {
            get => _currentSubState;
            private set
            {
                _currentSubState = value;
                CurrentSubStateView = _currentSubState.GetType().Name;
            }
        }
        public Player Player
        {
            get => _player;
        }
        public bool AutoPause
        {
            get => autoPause; 
            private set => autoPause = value;
        }
        public StateFactory StateFactory
        {
            get => _stateFactory;
        }

        #endregion

        private void Awake()
        {
            _stateFactory = new StateFactory(this);
            _player = GetComponent<Player>();
            
            // TODO: Set CurrentState
            SetCurrentState(_stateFactory.GetGroundState());
            if(CurrentState != null)
            {
                Debug.Log("Current State is not null");
                CurrentState.Enter();
            }
            else
            {
                Debug.LogError("<color=red><b>Current State is null</b></color>");
                CurrentStateView = "Current State is null";
                CurrentSubStateView = "Current Sub State is null";
            }
            
            StateList = _stateFactory.States.Values.Select(state => state.GetType().Name).ToList();
        }

        public void Update()
        {
            CurrentState?.UpdateStates();
        }

        public void SetCurrentState(BaseState currentState)
        {
            CurrentState = currentState;
        }
        public void SetCurrentSubState(BaseState currentSubState)
        {
            CurrentSubState = currentSubState;
        }

        public bool IsIdle()
        {
            return CurrentSubState == _stateFactory.GetIdleState();
        }
        
        public bool IsWalking()
        {
            return CurrentSubState == _stateFactory.GetWalkState();
        }
        
        public bool IsJumping()
        {
            return CurrentState == _stateFactory.GetJumpState();
        }
    }
}