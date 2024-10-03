using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;
using Utilities;
using Action = BehaviorDesigner.Runtime.Tasks.Action;

namespace AI.Vegnefly
{
    public class VegneflyPatrol : Action
    {
        [SerializeField] private TargetDetection targetDetection;
        [SerializeField] private float patrolRange = 3f;
        [SerializeField] private float patrolSpeed = 1f;

        [Header("Debug")]
        [SerializeField] private Vector3 originPosition;
        [SerializeField] private Vector2 targetPosition;
        
        private Collider2D collider;
        private Animator anim;

        private Vector2 dir;

        public override void OnAwake()
        {
            SetOriginPosition();
            FindNewPosition();
            
            collider = GetComponent<Collider2D>();
            anim = GetComponent<Animator>();
            
            targetDetection.OnPlayerDetectedStateChanged += SetOriginPosition;
        }

        public override TaskStatus OnUpdate()
        {
            if (collider.IsTouchingLayers() || (Vector2)transform.position == targetPosition)
            {
                FindNewPosition();
            }
            else
            {
                if (transform.localScale.x != -Mathf.Sign(dir.x))
                {
                    anim.SetTrigger(EnemyAnimationString.Turn);
                    AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
                    transform.localScale =
                        new Vector3(-Mathf.Sign(dir.x), transform.localScale.y, transform.localScale.z);
                }

                transform.position = Vector3.MoveTowards(transform.position, targetPosition, patrolSpeed * Time.deltaTime);
            }
            return TaskStatus.Running;
        }
        
        private void FindNewPosition()
        {
            //TODO: Vegvefly always flies to the reverse direction
            targetPosition = Utils.GetRandomPointInCircleReverseX(originPosition, patrolRange, transform.localScale.x);
            dir = Utils.GetDirectionVector2(transform.position, targetPosition);
        }

        private void SetOriginPosition()
        {
            originPosition = transform.position;
            FindNewPosition();
        }

        public override void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(originPosition, patrolRange);
                Gizmos.DrawSphere(targetPosition, 0.1f);
            }
        }
    }
}