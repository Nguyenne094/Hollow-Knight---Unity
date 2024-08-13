using UnityEngine;
using Utilities;

namespace Bap.State_Machine
{
    public class Jump : BaseState
    {
        public Jump(HierarchicalStateMachine ctx, StateFactory factory) : base(ctx, factory)
        {
            _isRoot = true;
        }

        public override void Enter()
        {
            InitializeSubState();
            _ctx.Player.Animator.SetBool(PlayerAnimationString.IsGrounded, false);
            _ctx.Player.Animator.SetBool(PlayerAnimationString.Jump, true);
        }

        public override void UpdateState()
        {
            CheckSwitchState();
        }

        public override void Exit()
        {
            _ctx.Player.Animator.SetBool(PlayerAnimationString.Jump, false);
            _ctx.Player.Animator.SetBool(PlayerAnimationString.IsGrounded, _ctx.Player.DirectionChecker.IsGrounded);
        }

        protected override void CheckSwitchState()
        {
            if (_ctx.Player.Rb.velocityY < -0.01f)
            {
                SwitchState(_factory.GetFallState());
            }
            else if(_ctx.Player.DirectionChecker.IsGrounded)
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