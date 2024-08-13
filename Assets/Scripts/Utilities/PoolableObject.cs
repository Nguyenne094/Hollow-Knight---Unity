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

        private void OnDisable()
        {
            Parent.Release(this.gameObject);
            OnRelease?.Invoke();
        }
    }
}