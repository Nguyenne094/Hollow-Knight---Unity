using System;
using System.Collections;
using Cinemachine;
using HealthSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace Nguyen.Camera
{
    public class CameraBrain2D : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private PlayerHealth playerHealth;
        [FormerlySerializedAs("hurtNoise")]
        [Header("Noise Settings")]
        [SerializeField] private NoiseSettings hurtNoise1;

        [SerializeField] private NoiseSettings hurtNoise2;
        [SerializeField] private float hurtNoiseDuration = 1;

        private void OnEnable()
        {
            //Passing hurtNoiseDuration for nothing
            playerHealth.OnTakeDamage += (float _) => StartCoroutine(StartNoise());
        }

        private void OnDisable()
        {
            playerHealth.OnTakeDamage -= (float _) => StartCoroutine(StartNoise());
        }

        private IEnumerator StartNoise()
        {
            Debug.Log("Start Noise");
            var noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            noise.m_NoiseProfile = hurtNoise1;
            yield return new WaitForSeconds(hurtNoiseDuration);
            noise.m_NoiseProfile = hurtNoise2;
            yield return new WaitForSeconds(hurtNoiseDuration / 2);
            noise.m_NoiseProfile = null;
        }
    }
}