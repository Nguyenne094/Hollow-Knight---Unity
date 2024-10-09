using UnityEditor;
using UnityEngine;
using Utilities;

namespace Bap.State_Machine
{
    public class Jump : BaseState
    {
        //The duration that Player presses
        private float holdingTime = 0f;
        //If Player just presses quickly, auto set holding time equal 0.15
        private float minHoldingTime;
        private float timeToPeak;
        private float gravity = -9.8f;
        private bool isJumping = false;
        private bool peak = false;
        private float jumpForce;
        
        public Jump(HierarchicalStateMachine ctx, StateFactory factory) : base(ctx, factory)
        {
            _isRoot = true;
            jumpForce = Mathf.Sqrt(-2 * gravity * 3);
            minHoldingTime = 0.2f;
            timeToPeak = 0.3f;
        }

        public override void Enter()
        {
            holdingTime = 0f;
            peak = false;
            _ctx.Player.CanJump = false;
            
            InitializeSubState();
            _ctx.Player.Animator.SetBool(PlayerAnimationString.IsGrounded, false);
            _ctx.Player.Animator.SetBool(PlayerAnimationString.Jump, true);
        }

        public override void UpdateState()
        {
            isJumping = _ctx.Player.Rb.velocityY >= 0;
            JumpCalculation();
            CheckSwitchState();
        }

        public override void Exit()
        {
            // _ctx.Player.Rb.velocityY = 0;
            _ctx.Player.Animator.SetBool(PlayerAnimationString.Jump, false);
        }

        protected override void CheckSwitchState()
        {
            if (!isJumping)
            {
                SwitchState(_factory.GetFallState());
            }
            else if (_ctx.Player.AttackInput)
            {
                SwitchState(_factory.GetAttackState());
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
        
        private void JumpCalculation()
        {
            if (!peak)
            {
                if (_ctx.Player.JumpPress || holdingTime < minHoldingTime)
                {
                    holdingTime += Time.deltaTime;
                }

                if (!_ctx.Player.JumpPress && holdingTime > minHoldingTime)
                {
                    _ctx.Player.Rb.velocityY = 0;
                    peak = true;
                }
                else if (holdingTime > timeToPeak)
                {
                    peak = true;
                }
            }

            _ctx.Player.Rb.velocity = new Vector2(_ctx.Player.Rb.velocity.x, peak ? _ctx.Player.Rb.velocityY * 0.8f : jumpForce);
        }
    }
}