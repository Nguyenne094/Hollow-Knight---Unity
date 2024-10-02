using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class SimpleWaterSpawner : MonoBehaviour
{
    [SerializeField] private GameObject waterInstance;

    private float dripDelay = 5;
    private float dripTimeOut = 0.1f;
    private GameObject water;

    private void Awake()
    {
        water = Instantiate(waterInstance, transform.position, Quaternion.identity);
        water.SetActive(false);
    }

    private void Update()
    {
        dripTimeOut -= Time.deltaTime;
        if (dripTimeOut < 0)
        {
            water.SetActive(true);
            water.transform.position = transform.position;
            dripDelay = Random.Range(5, 8);
            dripTimeOut = dripDelay;
        }
    }
}