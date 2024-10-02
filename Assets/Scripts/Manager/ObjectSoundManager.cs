using System;
using HealthSystem;
using UnityEngine;

public class ObjectSoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float innerRange = 10f;
    [SerializeField] private float outerRange = 13f;

    private EnemyHealth enemyHealth;

    private void Awake()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Debug.LogError($"AudioSource of {gameObject.name} is currently null");
        }

        enemyHealth = GetComponent<EnemyHealth>();
        enemyHealth.OnDeath += () => audioSource.Stop();
    }

    private void OnDestroy()
    {
        enemyHealth.OnDeath -= () => audioSource.Stop();
    }

    private void Update()
    {
        HandleVolumeAndPanStereo();
    }

    /// <summary>
    /// Adjust volume and pan stereo based on Player position
    /// </summary>
    private void HandleVolumeAndPanStereo()
    {
        float disToPlayer = Vector2.Distance(transform.position, Player.Instance.transform.position);
        if (disToPlayer < innerRange)
        {
            audioSource.volume = 1;
            audioSource.panStereo = 0;
        }
        else if(disToPlayer >= innerRange && disToPlayer < outerRange)
        {
            float ratio = (disToPlayer - innerRange) / (outerRange - innerRange);
            audioSource.volume = 1-ratio;
            audioSource.panStereo = ratio * Mathf.Sign(transform.position.x - Player.Instance.transform.position.x);
        }
        else if(disToPlayer > outerRange)
        {
            audioSource.volume = 0;
            audioSource.panStereo = 1;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, innerRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, outerRange);
    }
}