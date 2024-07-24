using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using Utilities;

namespace AI.Enemy.Vengefly
{
    public class ChaseTarget : Action
    {
        [SerializeField] private SharedTransform target;
        [SerializeField] private SharedBool playerDetected;
        
        private Animator animator;

        public override void OnAwake()
        {
            animator = GetComponent<Animator>();
        }

        public override TaskStatus OnUpdate()
        {
            if (animator.GetBool(EnemyAnimationString.CanMove) && target.Value != null && playerDetected.Value)
            {
                transform.DOMove(target.Value.position, 5f, true).SetEase(Ease.InFlash);
            }
            return TaskStatus.Running;
        }
    }

}