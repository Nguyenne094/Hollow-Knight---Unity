using UnityEngine.InputSystem;
using Utilities;

namespace PlayerStates
{
    public class Walk : PlayerState
    {
        public Walk(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
            
        }

        public override void Enter()
        {
            playerStateMachine.Player.Animator.SetBool(PlayerAnimationString.Walk, true);
        }

        public override void Update()
        {
            if (playerStateMachine.Player.MoveInput.x == 0)
            {
                playerStateMachine.TransitionTo(playerStateMachine.Idle);
            }
        }

        public override void Exit()
        {
            
        }
    }
}