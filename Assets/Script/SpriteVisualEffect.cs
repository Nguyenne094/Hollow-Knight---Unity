using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class SpriteVisualEffect : MonoBehaviour
{
    public float scaleValue = 0.5f;
    public float effectDuration = 0.2f;
    public Ease easeType;
    [SerializeField] private List<Transform> children;

    private void Awake()
    {
        children = gameObject.GetComponentsInChildren<Transform>().Where(t => t != transform).ToList();
    }

    private void OnEnable()
    {
        foreach (var child in children)
        {
            child.DOScaleX(child.localScale.x * scaleValue, effectDuration).SetEase(easeType);
            child.GetComponent<SpriteRenderer>().DOFade(0, effectDuration);
        }
    }
}