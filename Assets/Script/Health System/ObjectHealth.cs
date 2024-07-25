using System;
using UnityEngine;

namespace HealthSystem
{
    public class ObjectHealth : MonoBehaviour, IDamageable
    {
        [SerializeField] private float maxHealth;
        [SerializeField] private float currentHealth;
        [SerializeField] private ParticleSystem destructionParticleSystem;
        [SerializeField] private LayerMask attackableLayer;
        
        public float MaxHealth { get => maxHealth; private set => maxHealth = value; }
        public float CurrentHealth { get => currentHealth; private set => currentHealth = value; }
        
        public event Action<float> OnTakeDamage;
        public event Action OnDeath;

        private void Start()
        {
            CurrentHealth = MaxHealth;
        }

        private void OnEnable()
        {
            OnTakeDamage += TakeDamage;
            OnDeath += Die;
        }

        private void OnDisable()
        {
            OnTakeDamage -= TakeDamage;
            OnDeath -= Die;
        }

        private void Die()
        {
            if (destructionParticleSystem != null)
            {
                destructionParticleSystem.transform.parent = null;
                destructionParticleSystem.gameObject.SetActive(true);
                
                Destroy(this.gameObject);
            }
        }

        public void TakeDamage(float damage)
        {
            if (CurrentHealth > 0)
            {
                CurrentHealth -= damage;
                if (CurrentHealth == 0)
                {
                    OnDeath?.Invoke();
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Attackable attackable = other.gameObject.GetComponent<Attackable>();
            if (attackable != null && other.CompareTag("Weapon"))
            {
                Debug.Log( this.gameObject.name + " Take Damage!");
                OnTakeDamage?.Invoke(1f);
            }
            else
            {
                Debug.Log($"{other.gameObject.name} is not attackable!\n {attackable == null}");
            }
        }
    }
}