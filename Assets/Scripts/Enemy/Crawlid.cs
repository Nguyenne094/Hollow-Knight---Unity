using System;
using DG.Tweening;
using HealthSystem;
using UnityEngine;
using Utilities;

namespace Enemy
{
    public class Crawlid : MonoBehaviour
    {        
        [SerializeField] private BoxCollider2D checkBox;
        [SerializeField] private float speed = 5f;
        [SerializeField] private bool canMove = true; //just for debugging
        [SerializeField] private bool isFacingRight = true;

        private Rigidbody2D rb;
        private BoxCollider2D collider;
        private Animator animator;
        
        //! Dependencies
        private CliffDetection cliffDetection;
        private DirectionChecker directionChecker;
        private EnemyHealth enemyHealth;

        public bool IsFacingRight
        {
            get => isFacingRight;
            private set
            {
                if(isFacingRight != value)
                {
                    transform.localScale *= new Vector2(-1, 1);
                    isFacingRight = value; 
                }
            }
        }

        public bool CanMove
        {
            get
            {
                return animator.GetBool(EnemyAnimationString.CanMove);
            }
            private set
            {
                canMove = value;
            }
        }
        

        void Awake()
        {
            directionChecker = GetComponent<DirectionChecker>();
            cliffDetection = GetComponentInChildren<CliffDetection>();
            collider = GetComponent<BoxCollider2D>();
            enemyHealth = GetComponent<EnemyHealth>();
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {         
            //Check Direction
            if(directionChecker.IsGrounded && directionChecker.IsTouchingWall ||
            directionChecker.IsGrounded && cliffDetection.OnCliff)
            {
                FlipDirection();
                animator.SetTrigger(EnemyAnimationString.Turn);
            }
            
            //Move
            if(CanMove && enemyHealth.IsAlive)
            {
                rb.velocity = new Vector2(directionChecker.WallCheckDirection.x * speed, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocityY);
            }
        }

        private void FlipDirection()
        {
            if(IsFacingRight && (directionChecker.IsTouchingWall || cliffDetection.OnCliff))
            {
                IsFacingRight = false;
            }
            else if(!IsFacingRight && (directionChecker.IsTouchingWall || cliffDetection.OnCliff))
            {
                IsFacingRight = true;
            }
        }
    }
}