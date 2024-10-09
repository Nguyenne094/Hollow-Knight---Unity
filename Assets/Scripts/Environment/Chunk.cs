using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Chunk : MonoBehaviour
{
    private Collider2D collider;

    public event Action onTriggerEnter;
    public event Action onTriggerExit;
    
    private void Awake()
    {
        collider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            onTriggerEnter?.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            onTriggerExit?.Invoke();
        }
    }
}