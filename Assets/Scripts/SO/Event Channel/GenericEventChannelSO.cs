using System;
using UnityEngine;
using UnityEngine.Events;

namespace Nguyen.Event
{
    public abstract class GenericEventChannelSO<T> : ScriptableObject
    {
        public UnityAction<T> OnEventRaised;
        
        public void RaiseEvent(T value)
        {
            OnEventRaised?.Invoke(value);
        }
    }
}