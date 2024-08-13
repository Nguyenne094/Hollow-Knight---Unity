using BehaviorDesigner.Runtime;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

namespace AI.Enemy.Vengefly
{
    public class FindTarget : Action
    {
        
        [Header("Settings")]
        [SerializeField] private SharedTransform target;
        [SerializeField] private string tag;
        [SerializeField, Range(1, 20)] private float range = 5f;
        [SerializeField, Range(1, 20)] private float detectedRange = 10;

        [Header("DEBUG")] 
        [SerializeField] private SharedBool playerDetected = false;

        public bool PlayerDetected { get => playerDetected.Value; set => playerDetected = value; }
        
        private Rigidbody2D rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
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
            if (Vector2.Distance(transform.position, target.Value.position) < range)
            {
                playerDetected.SetValue(true);
                return TaskStatus.Success;
            }
            else if (Vector2.Distance(transform.position, target.Value.position) >
                     (playerDetected.Value ? detectedRange : range))
            {
                playerDetected.SetValue(false);
                return TaskStatus.Failure;
            }

            return TaskStatus.Failure;
        }
    }
}