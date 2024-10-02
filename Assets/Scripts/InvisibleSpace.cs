using System;
using DG.Tweening;
using Utilities;
using UnityEngine;

public class InvisibleSpace : MonoBehaviour
{
    [SerializeField] private Transform targetToTrigger;
    [SerializeField] private float fadeDuration = 1f;
    
    private void OnEnable()
    {
        targetToTrigger.GetComponent<DestroyNotifier>().OnObjectDestroyed += FadeOut;
    }

    private void FadeOut(GameObject gameObject)
    {
        targetToTrigger.GetComponent<DestroyNotifier>().OnObjectDestroyed -= FadeOut;
        Debug.Log(gameObject.name + " is destroyed");
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.DOColor(new Color(0, 0, 0, 0), fadeDuration);
        Destroy(this.gameObject, fadeDuration);
    }
}