using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] private Transform MainCamera;
    private float ParallaxCoefficient => transform.position.z;
    private Vector3 lastCameraPosition;
    
    private void Start()
    {
        if (MainCamera == null)
        {
            MainCamera = Camera.main.transform;
            if (MainCamera == null)
            {
                Debug.LogError("<color: red>Camera not found</color>");
            }
        }

        lastCameraPosition = MainCamera.position;
    }

    private void Update()
    {
        if (MainCamera.position != lastCameraPosition)
        {
            Vector3 distanceDelta = MainCamera.position - lastCameraPosition;
            lastCameraPosition = MainCamera.position;
            transform.position = new Vector3(transform.position.x + (distanceDelta.x * ParallaxCoefficient), transform.position.y, transform.position.z);
        }
    }
}
