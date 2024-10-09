using UnityEngine;
using Utilities;

namespace Bap.State_Machine
{
    public class Ground : BaseState
    {
        public Ground(HierarchicalStateMachine ctx, StateFactory factory) : base(ctx, factory)
        {
            _isRoot = true;
        }

        public override void Enter()
        {
            InitializeSubState();
        }

        public override void UpdateState()
        {
            CheckSwitchState();
        }

        public override void Exit()
        {
            
        }

        protected override void CheckSwitchState()
        {
            //TODO: can switch to Jump, Fall, Attack, Hurt, Dash
            if (_ctx.Player.CanJump)
            {
                SwitchState(_factory.GetJumpState());
            }
            else if(_ctx.Player.Rb.velocityY < -0.01f)
            {
                SwitchState(_factory.GetFallState());
            }
            else if (_ctx.Player.AttackInput)
            {
                Debug.Log("Hello");
                SwitchState(_factory.GetAttackState());
            }
        }

        public override void InitializeSubState()
        {
            if (_ctx.Player.Rb.velocityX == 0)
            {
                SetSubState(_factory.GetIdleState());
            }
            else
            {
                SetSubState(_factory.GetWalkState());
            }
        }
    }
}