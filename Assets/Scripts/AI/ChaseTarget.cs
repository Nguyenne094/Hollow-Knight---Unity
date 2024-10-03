using BehaviorDesigner.Runtime;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Utilities;

namespace AI
{
    public class ChaseTarget : Action
    {
        [SerializeField] private float speed = 2f;
        [SerializeField] private SharedTransform target;
        [SerializeField] private SharedBool playerDetected;
        
        private Animator animator;
        private Rigidbody2D rb;

        public override void OnAwake()
        {
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            
            if(target == null)
                Debug.LogError("Target is null. Please assign a target to " + gameObject.name);
        }

        public override TaskStatus OnUpdate()
        {
            if (target == null) return TaskStatus.Failure;
            
            if (animator.GetBool(EnemyAnimationString.CanMove) && playerDetected.Value)
            {
                Vector2 dir = Utils.GetDirectionVector2(transform.position, target.Value.position);
                if (transform.localScale.x != -Mathf.Sign(dir.x))
                {
                    transform.localScale = new Vector3(-Mathf.Sign(dir.x), transform.localScale.y, transform.localScale.z);
                    animator.SetTrigger(EnemyAnimationString.Turn);
                }
                rb.AddForce(dir * speed, ForceMode2D.Force);
                rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -speed, speed), Mathf.Clamp(rb.velocity.y, -speed, speed));
            }
            return TaskStatus.Running;
        }
    }
}