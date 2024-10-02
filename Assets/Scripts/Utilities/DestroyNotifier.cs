using System;
using UnityEngine;

namespace Utilities
{
    public class DestroyNotifier : MonoBehaviour
    {
        public event Action<GameObject> OnObjectDestroyed;

        public void OnDestroy()
        {
            OnObjectDestroyed?.Invoke(this.gameObject);
        }
    }
}