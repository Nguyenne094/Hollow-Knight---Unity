using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent((typeof(Collider2D)))]
public class CliffDetection : MonoBehaviour
{
    [SerializeField] private bool onCliff = false;
    [SerializeField] private List<Collider2D> detectedColliders;

    void Start()
    {
        detectedColliders = new();
    }

    public bool OnCliff { get => onCliff; private set => onCliff = value; }
    
    private void  OnTriggerExit2D(Collider2D other)
    {
        detectedColliders.Remove(other);
        if(detectedColliders.Count == 0){
            OnCliff = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        detectedColliders.Add(other);
        if(detectedColliders.Count > 0){
            OnCliff = false;
        }
    }
}
