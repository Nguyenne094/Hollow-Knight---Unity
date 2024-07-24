using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using Utilities;

namespace HealthSystem
{
    public class EnemyHealth : Health
    {
        protected override IEnumerator HurtEffect()
        {
            isInvincible = true;
            
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            Animator animator = GetComponent<Animator>();
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            
            float timePerFlash = colorFlashDuration / colorFlashTimes;
            
            for (int i = 0; i < colorFlashTimes; i++)
            {
                sprite.color = hurtColor;
                yield return new WaitForSeconds(timePerFlash / 2);
                sprite.color = Color.white;
                yield return new WaitForSeconds(timePerFlash / 2);
            }
    
            animator.SetBool(EnemyAnimationString.CanMove, true);
            isInvincible = false;
            yield return null;
        }
    }
}