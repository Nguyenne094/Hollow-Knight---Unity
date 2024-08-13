using UnityEngine.InputSystem;
using Utilities;

namespace Bap.State_Machine
{
    public class Idle : BaseState
    {
        public Idle(HierarchicalStateMachine ctx, StateFactory factory) : base(ctx, factory)
        {
        }

        public override void Enter()
        {
            _ctx.Player.Animator.SetBool(PlayerAnimationString.Idle, true);
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
            if (_ctx.Player.Rb.velocityX != 0)
            {
                SwitchState(_factory.GetWalkState());
            }
        }

        public override void InitializeSubState()
        {
            
        }
    }
}