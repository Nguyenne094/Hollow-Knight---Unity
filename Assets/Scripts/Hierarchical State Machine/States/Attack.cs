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
            _isRoot = true;
        }

        public override void Enter()
        {
            Debug.Log("Attakc");
            InitializeSubState();
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
            if (_ctx.IsJumping)
            {
                SwitchState(_factory.GetJumpState());
            }
            else if(!_ctx.IsJumping && !_ctx.Player.DirectionChecker.IsGrounded)
            {
                SwitchState(_factory.GetFallState());
            }
            else if (_ctx.Player.DirectionChecker.IsGrounded)
            {
                SwitchState(_factory.GetGroundState());
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