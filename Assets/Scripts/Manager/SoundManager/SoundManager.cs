using System.Collections;
using System.Collections.Generic;
using Script.SO;
using UnityEngine;
using UnityEngine.Pool;
using Utilities;

/// <summary>
/// Play sound effect using object pool. Useful for playing lots of sound effects at the same time.
/// </summary>
public class SoundManager : Singleton<SoundManager>
{ 
 private AudioSource audioSource;
 private Dictionary<GameObject, ObjectPool<GameObject>> soundPoolDictionary = new Dictionary<GameObject, ObjectPool<GameObject>>();
 
 private void Awake()
 {
  audioSource = GetComponent<AudioSource>();
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