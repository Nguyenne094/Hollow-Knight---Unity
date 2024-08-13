using UnityEngine.InputSystem;
using Utilities;
using UnityEngine;
using Bap.State_Machine;

namespace Bap.State_Machine
{
    public class Walk : BaseState
    {
        public override void Enter()
        {
            _ctx.Player.Animator.SetBool(PlayerAnimationString.Idle, false);
            _ctx.Player.Animator.SetBool(PlayerAnimationString.Walk, true);
        }

        public override void UpdateState()
        {
            CheckSwitchState();
        }

        public override void Exit()
        {
            _ctx.Player.Animator.SetBool(PlayerAnimationString.Walk, false);
        }

        protected override void CheckSwitchState()
        {
            if (_ctx.Player.Rb.velocity.x == 0)
            {
                SwitchState(_factory.GetIdleState());
            }
        }

        public override void InitializeSubState()
        {
            throw new System.NotImplementedException();
        }

        public Walk(HierarchicalStateMachine ctx, StateFactory factory) : base(ctx, factory)
        {
        }
    }
}