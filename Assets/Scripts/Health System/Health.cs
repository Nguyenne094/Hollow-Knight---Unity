using System;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using HealthSystem;
using Nguyen.Event;
using Unity.VisualScripting;
using UnityEngine.Serialization;
using Utilities;

namespace HealthSystem
{
    [RequireComponent(typeof(Animator))]
    public abstract class Health : MonoBehaviour, IDamageable
    {
        [Header("Health Settings")] 
        public bool isInvincible = false;
        [SerializeField] protected bool isAlive = true;
        [SerializeField] protected float maxHealth;
        [SerializeField] protected float currentHealth;

        [Header("Attack Settings")] 
        [SerializeField] protected Color hurtColor = Color.black;
        [SerializeField] protected float knockbackForce = 5;
        [SerializeField] private Vector2 knockbackDirection = new Vector2(5, 3);
        [SerializeField] protected float colorFlashDuration = 1;
        [SerializeField] protected uint colorFlashTimes = 3;

        [Header("Event Channel")]
        [SerializeField] private FloatEventChannelSO takeDamageEventChannelSo;
        
        [SerializeField] public GameObject contactedGameObject;
        [SerializeField] private LayerMask attackableLayer;

        //event
        public event Action<float> OnTakeDamage;
        public event Action OnDeath;
        
        public float MaxHealth { get => maxHealth; protected set => maxHealth = value; }
        public float CurrentHealth { get => currentHealth; protected set => currentHealth = value;}
        
        protected Animator animator;
        
        public bool IsAlive
        {
            get
            {
                return isAlive;
            }
            protected set
            {
                isAlive = value;
                // Both Player and enemies have the same animation parameter
                animator.SetBool("IsAlive", value);
                animator.SetBool("CanMove", value);
            }
        }

        private void OnEnable()
        {
            takeDamageEventChannelSo.OnEventRaised += OnEventRaised;
            OnTakeDamage += TakeDamage;
            OnDeath += Die;
        }

        private void OnDisable()
        {
            takeDamageEventChannelSo.OnEventRaised -= OnEventRaised;
            OnTakeDamage -= TakeDamage;
            OnDeath -= Die;
        }

        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
        }

        protected virtual void Start()
        {
            CurrentHealth = MaxHealth;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == attackableLayer)
            {
                contactedGameObject = other.gameObject;
            }
            
            Attackable attackable = other.GetComponent<Attackable>();
            if (attackable != null && IsAlive && !isInvincible)
            {
                OnTakeDamage?.Invoke(attackable.Damage);
            }
            
            DOVirtual.DelayedCall(0.1f, () => contactedGameObject = null);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if(other.gameObject == contactedGameObject)
                contactedGameObject = null;
        }

        public virtual void TakeDamage(float damage)
        {
            #region Subtract Health Logic

            Rigidbody2D rb = GetComponent<Rigidbody2D>();

            float takenDamage = Mathf.Min(damage, CurrentHealth);
            CurrentHealth -= takenDamage;
            print($"{gameObject.name} takes: {takenDamage} damage \nCurrentHealth: {CurrentHealth}");
        
            animator.SetTrigger(EnemyAnimationString.Hurt);

            #endregion

            #region Effects

            //* Knockback effect
            KnockbackEffect(rb);
            //* Play color flash effect
            StartCoroutine(HurtEffect());

            #endregion

            #region Death Logic

            if(CurrentHealth == 0) 
                OnDeath?.Invoke();

            #endregion
        }

        public void Die()
        {
            IsAlive = false;
            Destroy(this.gameObject, colorFlashDuration);
            Debug.Log($"{gameObject.transform.name} DIE");
        }

        protected void KnockbackEffect(Rigidbody2D rb)
        {
            if (contactedGameObject == null)
            {
                rb.velocity = new Vector2(knockbackDirection.x, rb.velocity.y + knockbackDirection.y);    
            }
            else
            {
                Transform contactedTarget = contactedGameObject.transform;
                float direction = Mathf.Sign(transform.position.x - contactedTarget.position.x);
                rb.velocity = new Vector2(knockbackDirection.x * direction, rb.velocity.y + knockbackDirection.y);
            }
        }

        protected abstract IEnumerator HurtEffect();

        protected void OnEventRaised(float damage)
        {
            OnTakeDamage?.Invoke(damage);
        }
    }
}