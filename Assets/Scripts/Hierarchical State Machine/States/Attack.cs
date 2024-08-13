using System;
using Bap.State_Machine;
using UnityEngine;
using Bap.State_Machine;
using Utilities;

namespace Bap.State_Machine
{
     public class Attack : BaseState
    {
        public Attack(HierarchicalStateMachine ctx, StateFactory factory) : base(ctx, factory)
        {
            
        }

        public override void Enter()
        {
            if (_ctx.Player.MoveInput.y == 1)
            {
                _ctx.Player.Animator.SetTrigger(PlayerAnimationString.AttackUp);
            }
            else if (_ctx.Player.MoveInput.y == -1)
            {
                _ctx.Player.Animator.SetTrigger(PlayerAnimationString.AttackDown);
            }
            else
            {
                _ctx.Player.Animator.SetTrigger(PlayerAnimationString.AttackRight);
            }
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
            
        }

        public override void InitializeSubState()
        {
            if (_ctx.Player.Rb.velocity == Vector2.zero)
            {
                _ctx.SetCurrentSubState(_factory.GetIdleState());
            }
            else
            {
                _ctx.SetCurrentSubState(_factory.GetIdleState());
            }
        }
    }
}