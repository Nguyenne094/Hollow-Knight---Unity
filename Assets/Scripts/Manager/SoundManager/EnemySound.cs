using System;
using UnityEngine;
using HealthSystem;

namespace Manager
{
    public class EnemySound : SoundAbstract
    {
        [SerializeField] private AudioClip walkClip;
        private EnemyHealth enemyHealth;

        protected override void Awake()
        {
            base.Awake();
            audioSource.clip = walkClip;
            audioSource.loop = true;
            audioSource.Play();
        }

        private void OnEnable()
        {
            enemyHealth = GetComponent<EnemyHealth>();
            enemyHealth.OnDeath += () => audioSource.Stop();
        }
        
        private void OnDestroy()
        {
            enemyHealth.OnDeath -= () => audioSource.Stop();
        }
        
        private void Update()
        {
            HandleVolumeAndPanStereo(audioSource);
        }

        /// <summary>
        /// Adjust volume and pan stereo based on Player position
        /// </summary>
    }
}