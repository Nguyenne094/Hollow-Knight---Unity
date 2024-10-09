using UnityEngine;
using Utilities;

namespace Bap.State_Machine
{
    public class Fall : BaseState
    {
        public Fall(HierarchicalStateMachine ctx, StateFactory factory) : base(ctx, factory)
        {
            _isRoot = true;
        }

        public override void Enter()
        {
            InitializeSubState();
            _ctx.Player.Rb.gravityScale = 2.5f;
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
            if(_ctx.Player.DirectionChecker.IsGrounded)
            {
                _ctx.Player.Rb.gravityScale = 1;
                SwitchState(_factory.GetGroundState());
            }
            else if (_ctx.Player.AttackInput)
            {
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