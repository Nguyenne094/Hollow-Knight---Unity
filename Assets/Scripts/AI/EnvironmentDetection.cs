using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Utilities;

namespace AI
{
    public class EnvironmentDetection : Conditional
    {
        [SerializeField] private SharedTransform target;
        [SerializeField] ContactFilter2D obstacleFilter;
        [SerializeField] private bool isObstacleDetected;
        
        public bool IsObstacleDetected => isObstacleDetected;

        public override void OnAwake()
        {
            if(target == null)
                Debug.LogError("Target is null. Please assign a target to " + gameObject.name);
        }

        public override TaskStatus OnUpdate()
        {
            Vector2 dir = Utils.GetDirectionVector2(transform.position, target.Value.position);
            if (Physics2D.Raycast(transform.position, dir, obstacleFilter, new RaycastHit2D[2]) > 0)
            {
                isObstacleDetected = true;
                return TaskStatus.Success;
            }
            else
            {
                isObstacleDetected = false;
                return TaskStatus.Failure;
            }
        }
    }
}