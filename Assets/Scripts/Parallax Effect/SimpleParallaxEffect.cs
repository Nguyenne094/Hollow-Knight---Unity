using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class SimpleParallaxEffect : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Transform _subject;
    [SerializeField] private bool _applyX = true;
    [SerializeField] private bool _applyY = false;
    [SerializeField] private float _parallaxFactor;
    
    private Vector3 lastCameraPosition;
    private float disToSub => transform.position.z - _subject.transform.position.z;
    private float clippingPlan => _mainCamera.transform.position.z + (disToSub > 0 ? _mainCamera.farClipPlane : _mainCamera.nearClipPlane);
    private float parallaxFactor => Mathf.Abs(disToSub/clippingPlan);
    
    private void Start()
    {
        if (_mainCamera == null)
        {
            _mainCamera = Camera.main;
            if (_mainCamera == null)
            {
                Debug.LogError("<color: red>Camera not found</color>");
            }
        }

        lastCameraPosition = _mainCamera.transform.position;
    }

    private void Update()
    {
        _parallaxFactor = parallaxFactor;
        if (_mainCamera.transform.position == lastCameraPosition) return;
        else
        {
            Vector3 travel = _mainCamera.transform.position - lastCameraPosition;
            transform.position = new Vector3(transform.position.x + (_applyX ? (travel.x * parallaxFactor) : 0), transform.position.y + (_applyY ? (travel.y * parallaxFactor) : 0), transform.position.z);
            
            lastCameraPosition = _mainCamera.transform.position;
        }
    }
}
