using System;
using System.Collections;
using System.Collections.Generic;
using Script.SO;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;
using Utilities;
using Random = UnityEngine.Random;

public class PlayerSoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip footStepSound;
    [SerializeField, Range(0, 1)] private float footStepSoundVolume = .2f;
    [Space, Header("Jump Sound")]
    [SerializeField] private SoundEffect jumpSoundEffect;
    [SerializeField] private SoundEffect landSoundEffect;
    [SerializeField] private AudioClip fallSoundEffect;
    [SerializeField, Range(0, 1)] private float fallSoundVolume = .2f;
    [Space] [SerializeField] private SoundEffect attackSoundEffect;
    [SerializeField] private AudioClip damageSound;
    [SerializeField, Range(0, 1)] private float damageSoundVolume = .2f;
   [SerializeField] private SoundEffect hurtSoundEffect;
    
    private AudioSource audioSource;
    private Dictionary<GameObject, ObjectPool<GameObject>> soundPoolDictionary = new Dictionary<GameObject, ObjectPool<GameObject>>();

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        PlayRunSound();
    }

    private void PlayRunSound()
    {
        if (!audioSource.isPlaying && Player.Instance.PlayerStateMachine.IsWalking() && Player.Instance.DirectionChecker.IsGrounded)
        {
            audioSource.PlayOneShot(footStepSound, footStepSoundVolume);
        }
        else if (audioSource.isPlaying && !Player.Instance.PlayerStateMachine.IsWalking() || audioSource.isPlaying && !Player.Instance.DirectionChecker.IsGrounded)
        {
            audioSource.Stop();
        }
    }

    private void PlayFallSound()
    {
        if (!audioSource.isPlaying && Player.Instance.PlayerStateMachine.IsJumping() && Player.Instance.Rb.velocityY < 0)
        {
            audioSource.PlayOneShot(fallSoundEffect, fallSoundVolume);
        }
        else if(audioSource.isPlaying && Player.Instance.PlayerStateMachine.IsJumping() && Player.Instance.Rb.velocityY >= 0)
        {
            audioSource.Stop();
        }
    }

    public void PlayJumpSound()
    {
        PlaySoundEffect(jumpSoundEffect);
    }

    public void PlayLandSound()
    {
        PlaySoundEffect(landSoundEffect);
    }

    public void PlayAttackSound()
    {
        PlaySoundEffect(attackSoundEffect);
    }

    public void PlayHurtSound()
    {
        PlaySoundEffect(hurtSoundEffect);
    }

    public void PlaySoundEffect(SoundEffect soundEffect)
    {
        GameObject audioPref = soundEffect.AudioSourcePref.gameObject;
        if (!soundPoolDictionary.ContainsKey(audioPref))
        {
            soundPoolDictionary.Add(audioPref, new ObjectPool<GameObject>(() => Instantiate(audioPref)));
        }

        GameObject instance = soundPoolDictionary[audioPref].Get();
        if (instance.TryGetComponent(out PoolableObject poolableObject))
        {
            poolableObject.Parent = soundPoolDictionary[audioPref];
            poolableObject.IsReleased = false;
        }
        instance.transform.position = transform.position;
        instance.SetActive(true);
        AudioClip randomClip = soundEffect.AudioClips[Random.Range(0, soundEffect.AudioClips.Count)];
        
        AudioSource audioSource = instance.GetComponent<AudioSource>();
        audioSource.PlayOneShot(randomClip, Random.Range(soundEffect.VolumeRange.x, soundEffect.VolumeRange.y));
        StartCoroutine(DisableAudioSource(instance, randomClip.length));
    }

    private IEnumerator DisableAudioSource(GameObject audioSource, float clipLength)
    {
        yield return new WaitForSeconds(clipLength);
        audioSource.SetActive(false);
    }
}