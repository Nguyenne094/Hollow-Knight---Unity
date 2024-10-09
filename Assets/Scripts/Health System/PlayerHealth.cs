using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;
using Utilities;

namespace HealthSystem
{
    public class PlayerHealth : Health
    {
        [Header("Player Health Settings")]
        [SerializeField] private uint freezeFramesWhenHurt = 3;
        [SerializeField] private ParticleSystem hurtParticlePref;
        [SerializeField] private SpriteVisualEffect hurtSpriteEffectPref;
        
        private IObjectPool<ParticleSystem> hurtParticlePool;
        private PlayerSoundManager soundManager;

        protected override void Awake()
        {
            soundManager = GetComponent<PlayerSoundManager>();
            hurtParticlePool = new ObjectPool<ParticleSystem>(CreateHurtParticle, OnGetHurtParticle, OnReleaseHurtParticle, OnDestroyHurtParticle, true, 5, 5);
            base.Awake();
        }

        #region HurtParticle Pool's Methods

        private void OnDestroyHurtParticle(ParticleSystem hurtParticle)
        {
            //* Destroy particle after lifetime
            if (hurtParticle == null) return;
            else
            {
                hurtParticle.Stop();
                Destroy(hurtParticle);
            }
        }

        private void OnReleaseHurtParticle(ParticleSystem hurtParticle)
        {
            //Don't need set active false because when ParticleSystem has done, it automatically disable itself.
            //hurtParticle.gameObject.SetActive(false);
        }

        private void OnGetHurtParticle(ParticleSystem hurtParticle)
        {
            hurtParticle.gameObject.SetActive(true);
            hurtParticle.transform.position = transform.position;
            hurtParticle?.Stop();
            hurtParticle?.Play();
        }

        private ParticleSystem CreateHurtParticle()
        {
            return Instantiate(hurtParticlePref);
        }

        #endregion

        protected override IEnumerator HurtEffect()
        {
            isInvincible = true;
            
            //* Play hurt VISUAL EFFECT
            ParticleSystem hurtParticleInstance = hurtParticlePool.Get();

            if (hurtParticlePref != null)
            {
                SpriteVisualEffect hurtSpriteInstance = Instantiate(hurtSpriteEffectPref, transform.position, Quaternion.identity);
            }

            //* Play hurt SOUND EFFECT
            soundManager.PlayHurtSound();
            
            StartCoroutine(FreezeFrame(freezeFramesWhenHurt));
            
            //* COLOR SPLASH EFFECT
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            float timePerFlash = colorFlashDuration / colorFlashTimes;
            for (int i = 0; i < colorFlashTimes; i++)
            {
                sprite.color = hurtColor;
                yield return new WaitForSeconds(timePerFlash / 2);
                sprite.color = Color.white;
                yield return new WaitForSeconds(timePerFlash / 2);
            }

            yield return new WaitForSeconds(hurtParticlePref.main.startLifetime.constant);
            hurtParticlePool.Release(hurtParticleInstance);
            isInvincible = false;
        }
        
        private IEnumerator FreezeFrame(uint frames)
        {
            Time.timeScale = 0;
            for(int i = 0; i < frames; i++)
            {
                yield return null;
            }
            Time.timeScale = 1;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            Attackable attackable = other.GetComponent<Attackable>();
            if (attackable != null && other.CompareTag("Enemy") && IsAlive && !isInvincible)
            {
                OnEventRaised(attackable.Damage);
            }
        }
    }
}