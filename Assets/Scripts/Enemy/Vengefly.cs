using HealthSystem;
using UnityEngine;
using Utilities;

public class Vengefly : MonoBehaviour
{
    private BoxCollider2D collider;
    private Animator animator;
    private EnemyHealth enemyHealth;
    private SimplePathfinding pathfinding;
    
    private void OnEnable()
    {
        enemyHealth.OnDeath += Die;
    }
    
    private void OnDisable()
    {
        enemyHealth.OnDeath -= Die;
    }
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
    }

    private void Die()
    {
        animator.SetBool(EnemyAnimationString.CanMove, false);
        animator.SetBool(EnemyAnimationString.IsAlive, false);
    }
}
