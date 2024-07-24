using System;
using System.Collections;
using System.Collections.Generic;
using PlayerStates;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using HealthSystem;
using Utilities;

[RequireComponent(typeof(DirectionChecker))]
public class Player : Utilities.Singleton<Player>
{

    [Header("State Information")] public string CurrentState;
    
    [Header("Movement Settings")]
    [SerializeField, Range(0.1f, 100)] private float speed = 5f;

    [Header("Jump Settings")] 
    [SerializeField] private float gravity = 9.82f;
    [SerializeField] private float fallingGravityMultiplier = 2f;
    
    [Header("Debug")]
    [SerializeField] private Vector2 moveInput;
    [SerializeField] private bool canMove = true;
    [SerializeField] private bool canJump = true;
    
    private float jumpHoldingTime = 0;
    public bool jumpPress = false;
    private bool isFacingRight = true;
    
    private new BoxCollider2D collider;
    private Rigidbody2D rb;
    private Animator animator;
    private ContactFilter2D contactFilter;
    
    //! Dependencies
    private PlayerHealth playerHealth;
    private DirectionChecker directionChecker;
    private PlayerStateMachine playerStateMachine;
    
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
    
    public PlayerStateMachine PlayerStateMachine { get => playerStateMachine; set => playerStateMachine = value; }
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
    }
    
    private void Start()
    {
        playerStateMachine = new PlayerStateMachine(this);
        playerStateMachine.Initialize(playerStateMachine.Idle);
        playerStateMachine.OnStateChanged += StateChanged;
        
        CurrentState = playerStateMachine.CurrentState.ToString();
    }

    private void OnDisable()
    {
        playerStateMachine.OnStateChanged -= StateChanged;
    }
    
    private void FixedUpdate()
    {
        if (!LockVelocity)
        {
            rb.velocity = new Vector2(MoveInput.x * speed, rb.velocity.y);
        }
        rb.gravityScale = rb.velocity.y < -0.1f ? Mathf.Lerp(rb.gravityScale, fallingGravityMultiplier, 10 * Time.deltaTime) : 1;
        JumpCalculation();
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
        playerStateMachine.Tick();
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

    public void Attack(InputAction.CallbackContext ctx)
    {
        if(ctx.started)
        {
            if (MoveInput.y == 1)
            {
                animator.SetTrigger(PlayerAnimationString.AttackUp);
            }
            else if (MoveInput.y == -1 && rb.velocity.y < 0)
            {
                animator.SetTrigger(PlayerAnimationString.AttackDown);
            }
            else
            {
                animator.SetTrigger(PlayerAnimationString.AttackRight);
            }
        }
    }

    public void StateChanged()
    {
        CurrentState = playerStateMachine.CurrentState.ToString();
    }
}