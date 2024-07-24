namespace PlayerStates
{
    public class Attack : PlayerState
    {
        public Attack(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }

        public override void Enter()
        {
            // playerStateMachine.Player.Animator.SetTrigger(AnimationString.Attack);
        }

        public override void Update()
        {
            if (playerStateMachine.Player.MoveInput.x == 0)
            {
                playerStateMachine.TransitionTo(playerStateMachine.Idle);
            }
            else
            {
                playerStateMachine.TransitionTo(playerStateMachine.Walk);
            }
        }

        public override void Exit()
        {
            
        }
    }
}