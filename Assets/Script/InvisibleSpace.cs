using System;
using DG.Tweening;
using Unity.VisualScripting;
using Utilities;
using UnityEngine;

public class InvisibleSpace : MonoBehaviour
{
    [SerializeField] private Transform targetToTrigger;
    [SerializeField] private float fadeDuration = 1f;

    private void OnEnable()
    {
        DestroyNotifier.OnObjectDestroyed += FadeOut;
    }
    
    private void OnDisable()
    {
        DestroyNotifier.OnObjectDestroyed -= FadeOut;
    }

    private void FadeOut(GameObject gameObject)
    {
        Debug.Log(gameObject.name + " is destroyed");
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.DOColor(new Color(0, 0, 0, 0), fadeDuration);
        Destroy(this.gameObject, fadeDuration);
    }
}