using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;

namespace Utilities
{
    /// <summary>
    /// GameObject pools itself when disabled.
    /// </summary>
    public class PoolableObject : MonoBehaviour
    {
        public ObjectPool<GameObject> Parent;
        public event Action OnRelease;

        private bool isReleased;
        public bool IsReleased { get => isReleased; set => isReleased = value; }

        private void OnDisable()
        {
            if (!IsReleased)
            {
                Parent.Release(this.gameObject);
                OnRelease?.Invoke();
                IsReleased = true;
            }
        }

        private void OnDestroy()
        {
            if (!IsReleased)
            {
                Parent.Release(this.gameObject);
                OnRelease?.Invoke();
                IsReleased = true;
            }
        }
    }
}