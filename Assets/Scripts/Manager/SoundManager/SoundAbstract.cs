using System;
using HealthSystem;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public abstract class SoundAbstract : MonoBehaviour
{
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected AudioClip[] triggerClips;
    [SerializeField] protected float innerRange = 10f;
    [SerializeField] protected float outerRange = 13f;
    [SerializeField] protected bool disableOnTrigger = false;
    
    protected virtual void Awake()
    {
        if (audioSource == null)
        {
            audioSource = Instantiate(new GameObject()).AddComponent<AudioSource>();
        }
        else
        {
            audioSource = Instantiate(audioSource);
        }
    }
    
    protected void HandleVolumeAndPanStereo(AudioSource audioSource)
    {
        if (Player.Instance != null)   
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
        else
        {
            Debug.LogError("Player is not exist in the scene!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (disableOnTrigger && triggerClips.Length > 0 && other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            audioSource?.Stop();
            HandleVolumeAndPanStereo(audioSource);
            audioSource.PlayOneShot(triggerClips[Random.Range(0, triggerClips.Length)]);
            if (disableOnTrigger)
            {
                gameObject.SetActive(false);
            }
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