using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class SimpleParallaxEffect : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Chunk _belongToChunk;
    [SerializeField] private Transform _subject;
    [SerializeField] private bool _applyX = true;
    [SerializeField] private bool _applyY = false;
    [SerializeField] private float _parallaxFactor;

    private bool enableParallax = false;
    
    private Vector3 lastCameraPosition;
    private float disToSub => transform.position.z - _subject.transform.position.z;
    private float clippingPlan => Mathf.Abs(_mainCamera.transform.position.z + (disToSub > 0 ? _mainCamera.farClipPlane : _mainCamera.nearClipPlane));
    private float parallaxFactor => disToSub/clippingPlan;

    private void Awake()
    {
        if (_belongToChunk == null)
        {
            _belongToChunk = GetComponentInParent<Chunk>();
            if(_belongToChunk == null)
                Debug.LogError("Require Chunk script in the parent GameObject");
        }
    }

    private void OnEnable()
    {
        _belongToChunk.onTriggerEnter += () => enableParallax = true;
        _belongToChunk.onTriggerExit += () => enableParallax = false;
    }

    private void OnDisable()
    {
        _belongToChunk.onTriggerEnter -= () => enableParallax = true;
        _belongToChunk.onTriggerExit += () => enableParallax = false;
    }

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
        if (enableParallax)
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
}
