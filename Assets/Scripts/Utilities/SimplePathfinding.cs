using System;
using DG.Tweening;
using UnityEditor.UIElements;
using UnityEngine;

namespace Utilities
{
    public class SimplePathfinding : MonoBehaviour
    {
        private enum FindPathMethod
        {
            ByGameObject,
            ByTag
        }
        
        [Header("Settings")]
        [SerializeField] private FindPathMethod findPathMethod = FindPathMethod.ByGameObject;
        [SerializeField] private Transform target;
        [SerializeField] private string tag;
        [SerializeField, Range(1, 20)] private float range = 5f;
        [SerializeField, Range(1, 20)] private float detectedRange = 10;
        [SerializeField] private float speed = 5f;

        [Header("DEBUG")] 
        [SerializeField] private bool playerDetected = false;

        public bool PlayerDetected { get => playerDetected; set => playerDetected = value; }
        
        private Rigidbody2D rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            if (findPathMethod == FindPathMethod.ByTag)
            {
                target = GameObject.FindGameObjectWithTag(tag).transform;
            }

            if (target == null)
            {
                Debug.LogError($"Target is null. Please assign a target to {gameObject.name}");
            }
        }

        private void Update()
        {
            if (Vector2.Distance(transform.position, target.position) < range)
            {
                playerDetected = true;
            }
            else if (Vector2.Distance(transform.position, target.position) > detectedRange)
            {
                playerDetected = false;
            }
            if (Vector2.Distance(transform.position, target.position) < (playerDetected ? detectedRange : range))
            {
                transform.position = Vector2.Lerp(transform.position, target.position, speed);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, (playerDetected ? detectedRange : range));
            if (Vector2.Distance(transform.position, target.position) < (playerDetected ? detectedRange : range))
            {
                Gizmos.DrawLine(transform.position, target.position);
            }
        }
    }
}