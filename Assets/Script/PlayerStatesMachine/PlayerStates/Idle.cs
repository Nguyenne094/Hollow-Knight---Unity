using UnityEngine.InputSystem;
using Utilities;

namespace PlayerStates
{
    public class Idle : PlayerState
    {
        public Idle(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
            
        }

        public override void Enter()
        {
            playerStateMachine.Player.Animator.SetBool(PlayerAnimationString.Walk, false);
        }

        public override void Update()
        {
            if (playerStateMachine.Player.MoveInput.x != 0 && playerStateMachine.Player.DirectionChecker.IsGrounded)
            {
                playerStateMachine.TransitionTo(playerStateMachine.Walk);
            }
        }

        public override void Exit()
        {
            
        }
    }
}