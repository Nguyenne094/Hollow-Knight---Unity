using System;
using DG.Tweening;
using UnityEngine;

public class WaterDrip : MonoBehaviour
{
    [SerializeField] private GameObject waterInstance;
    [SerializeField] private bool isRandomWaterDrip;
    [SerializeField] private Vector2 randomRange;
    [SerializeField] private float fixedTimeDrip = 5f;

    private float dripTimeOut = 0;
    
    private void Update()
    {
        dripTimeOut += Time.deltaTime;
        if (dripTimeOut >= 5)
        {
            dripTimeOut = 0;
            waterInstance.transform.position = transform.position;
            waterInstance.SetActive(true);
        }
    }
}