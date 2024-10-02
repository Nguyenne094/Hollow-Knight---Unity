using System;
using System.Collections.Generic;
using Script.SO;
using UnityEditor;
using UnityEngine;

public class DestructibleStaticObject : MonoBehaviour
{
    [SerializeField] private Sprite destructedSprite;
    [SerializeField] private SoundEffect destructedSoundEffect;
    [SerializeField, Range(0, 1)] private float soundVolume = 0.5f;
    [SerializeField, Tooltip("The degree is applied into the Particle System when this object is destroyed from left or right")]
    private Vector2 destroyDegree = new Vector2(-45, -145);

    [SerializeField] private bool destroyObject;
    [SerializeField] private LayerMask attakerLayer;
    private bool destroyFromLeft;
    
    private ParticleSystem destructionParticleSystem;

    private void Awake()
    {
        destructionParticleSystem = GetComponentInChildren<ParticleSystem>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Weapon"))
        {
            //Destruct Sprite
            if(destructedSprite != null && !destroyObject)
            {
                GetComponent<SpriteRenderer>().sprite = destructedSprite;
            }
            else if(destroyObject)
            {
                destructionParticleSystem.gameObject.transform.SetParent(null);
                Destroy(gameObject);
            }
            //Destruct Direction
            destroyFromLeft = Player.Instance.transform.position.x < transform.position.x;
            var shapeModule = destructionParticleSystem.shape;
            shapeModule.rotation = new Vector3(destroyFromLeft ? destroyDegree.x : destroyDegree.y, shapeModule.rotation.y, shapeModule.rotation.z);
            //Play Particle System
            destructionParticleSystem.Play();
            //Play Sound
            SoundManager.Instance.PlaySoundEffect(destructedSoundEffect);
            //Remove this Script
            Destroy(this);
        }
    }
}