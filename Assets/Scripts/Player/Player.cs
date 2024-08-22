using System;
using System.Collections;
using System.Collections.Generic;
using Bap.State_Machine;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using HealthSystem;
using UnityEngine.Serialization;
using Utilities;

[RequireComponent(typeof(DirectionChecker))]
public class Player : Utilities.Singleton<Player>
{
    
    [Header("Movement Settings")]
    [SerializeField, Range(0.1f, 100)] private float speed = 5f;

    [Header("Jump Settings")] 
    [SerializeField] private float gravity = 9.82f;
    [SerializeField] private float yVelocityLimit = -10f;
    [SerializeField] private float fallingGravityMultiplier = 2f;
    
    [Header("Debug")]
    [SerializeField] private Vector2 moveInput;
    [SerializeField] private bool canMove = true;
    [SerializeField] private bool canJump = true;
    [SerializeField] private bool jumpPress = false;
    [SerializeField] private bool attackInput = false;
    [SerializeField] private bool isFacingRight = true;
    
    private float jumpHoldingTime = 0;
    
    private new BoxCollider2D collider;
    private Rigidbody2D rb;
    private Animator animator;
    private ContactFilter2D contactFilter;
    
    //! Dependencies
    private PlayerHealth playerHealth;
    private DirectionChecker directionChecker;
    private  HierarchicalStateMachine _playerStateMachine;
    
    public bool JumpPress { get => jumpPress; }
    public bool AttackInput { get => attackInput; }
    public Rigidbody2D Rb { get => rb; }
    public bool IsFacingRight 
    {
        get => isFacingRight;
        private set
        {
            if(isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }
            isFacingRight = value;
        }
        
    }
    public bool CanMove
    {
        get
        {
            return animator.GetBool(PlayerAnimationString.CanMove);
        }
        private set
        {
            canMove = value;
        }
    }
    public bool LockVelocity
    {
        get
        {
            return animator.GetBool(PlayerAnimationString.LockVelocity);
        }
    }
    
    public HierarchicalStateMachine PlayerStateMachine
    {
        get => _playerStateMachine;
    }
    public Animator Animator { get => animator; set => animator = value; }
    public Vector2 MoveInput { get => moveInput; private set => moveInput = value; }
    public DirectionChecker DirectionChecker { get => directionChecker; private set => directionChecker = value; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();
        directionChecker = GetComponent<DirectionChecker>();
        playerHealth = GetComponent<PlayerHealth>();
        _playerStateMachine = GetComponent<HierarchicalStateMachine>();
    }

    private void Update()
    {
        animator.SetFloat(PlayerAnimationString.yVelocity, rb.velocity.y);
        animator.SetBool(PlayerAnimationString.Idle, _playerStateMachine.IsIdle());
        animator.SetBool(PlayerAnimationString.IsGrounded, directionChecker.IsGrounded);
    }

    private void FixedUpdate()
    {
        rb.gravityScale = rb.velocity.y < -0.1f ? Mathf.Lerp(rb.gravityScale, fallingGravityMultiplier, 10 * Time.deltaTime) : 1;
        JumpCalculation();
        if (!LockVelocity)
        {
            rb.velocity = new Vector2(MoveInput.x * speed, (rb.velocityY < yVelocityLimit) ? yVelocityLimit : rb.velocity.y);
        }
    }

    private void JumpCalculation()
    {
        if (jumpPress && canJump)
        {
            jumpHoldingTime += Time.deltaTime * 2;
            if (jumpHoldingTime <= 0.2) jumpHoldingTime = 0.2f;
            else if (jumpHoldingTime >= 1)
            {
                jumpPress = false;
            }
            float jumpForce = Mathf.Sqrt(-2 * gravity * Easing.EaseOutCir(jumpHoldingTime));
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        else if (!jumpPress)
        {
            jumpHoldingTime = 0.01f;
        }

        if (rb.velocityY <= 0)
        {
            canJump = false;
        }
    }

    private void LateUpdate()
    {
        UpdateFacingDirection();
    }

    private void UpdateFacingDirection()
    {
        if (IsFacingRight && MoveInput.x < 0)
        {
            IsFacingRight = false;
        }
        else if(!IsFacingRight && MoveInput.x > 0)
        {
            IsFacingRight = true;
        }
    }
    
    public void Movement(InputAction.CallbackContext ctx)
    {
        if (playerHealth.IsAlive)
        {
            MoveInput = ctx.ReadValue<Vector2>();
        }
    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        if (ctx.started && directionChecker.IsGrounded)
        {
            canJump = true;
            jumpPress = true;
        }

        if (ctx.canceled && jumpPress)
        {
            jumpPress = false;
        }
    }

    private float attackTimeout = 0.4f;
    private float attackTime;
    public void Attack(InputAction.CallbackContext ctx)
    {
        if (ctx.started && attackTime == attackTimeout)
        {
            attackInput = true;
            attackTimeout = .4f;
            attackTime = attackTimeout;
            
            DOVirtual.DelayedCall(Time.deltaTime, () => attackInput = false);
            DOVirtual.Float(attackTime, 0, attackTimeout, (x) =>
            {
                attackTime = x;
                if (attackTime == 0)
                    attackTime = attackTimeout;
            });
        }
    }
}