using System;
using UnityEngine.Events;

namespace HealthSystem
{
    public interface IDamageable
    {
        public float MaxHealth { get;}
        public float CurrentHealth { get;}
        
        public event Action<float> OnTakeDamage;
        public event Action OnDeath;
        
        public void TakeDamage(float damage);
    }
}