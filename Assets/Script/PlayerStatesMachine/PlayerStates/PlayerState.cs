namespace PlayerStates
{
    public abstract class PlayerState : IState
    {
        protected PlayerStateMachine playerStateMachine;
        
        public PlayerState(PlayerStateMachine playerStateMachine)
        {
            this.playerStateMachine = playerStateMachine;
        }
        
        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit();
    }
}