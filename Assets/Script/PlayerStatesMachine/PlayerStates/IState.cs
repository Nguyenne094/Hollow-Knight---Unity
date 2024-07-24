namespace PlayerStates
{
   public interface IState
   {
      /// <summary>
      /// Set animation of this state here
      /// </summary>
      public void Enter();
      
      /// <summary>
      /// Check for conditions to transition to another state
      /// </summary>
      public void Update();
      
      /// <summary>
      /// 
      /// </summary>
      public void Exit();
   }
}