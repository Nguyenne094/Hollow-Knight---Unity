using System;
using UnityEngine;

namespace Utilities
{
    public class DestroyNotifier : MonoBehaviour
    {
        public static event Action<GameObject> OnObjectDestroyed;

        public void OnDestroy()
        {
            OnObjectDestroyed?.Invoke(this.gameObject);
        }
    }
}