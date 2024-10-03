using BehaviorDesigner.Runtime;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Utilities;
using System;
using Action = System.Action;

namespace AI
{
    public class TargetDetection : Conditional
    {
        [Header("Settings")]
        [SerializeField] private SharedTransform target;
        [SerializeField] private string tag;
        [SerializeField] private float normalRange = 5f;
        [SerializeField] private float detectedRange = 10;

        [Header("DEBUG")] 
        [SerializeField] private SharedBool playerDetected = false;

        public event Action OnPlayerDetectedStateChanged;

        private float Range => PlayerDetected ? detectedRange : normalRange;
        private Animator anim;
        private Rigidbody2D rb;

        public bool PlayerDetected { get => playerDetected.Value;
            set
            {
                anim.SetBool(EnemyAnimationString.Chase, value);
                if(playerDetected.Value != value) 
                    NotifyPlayerDetectedStateChanged();
                if (value == false)
                    rb.velocity = Vector3.zero;
                playerDetected.SetValue(value);
            }
        }

        public override void OnAwake()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
        }

        public override void OnStart()
        {
            if (target == null)
            {
                Debug.LogError($"Target is null. Please assign a target to {gameObject.name}");
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (target.Value == null)
                return TaskStatus.Failure;
            
            if (Vector2.SqrMagnitude(transform.position - target.Value.position) < Range * Range)
            {
                PlayerDetected = true;
                return TaskStatus.Success;
            }
            else if (Vector2.Distance(transform.position, target.Value.position) > Range)
            {
                PlayerDetected = false;
                return TaskStatus.Failure;
            }

            return TaskStatus.Failure;
        }

        public void NotifyPlayerDetectedStateChanged()
        {
            OnPlayerDetectedStateChanged?.Invoke();
        }
    }
}