using System;
using UnityEngine;
using DG.Tweening;

public class CameraFollowingObject : MonoBehaviour
{
    [SerializeField] private Transform parent;

    private void LateUpdate()
    {
        float parentFacingDirection = parent.localScale.x;
    }
}