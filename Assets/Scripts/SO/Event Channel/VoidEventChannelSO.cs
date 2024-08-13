using System;
using UnityEngine;
using UnityEngine.Events;

namespace Nguyen.Event
{
    [CreateAssetMenu(fileName = "Void Event", menuName = "Event Channel/Void", order = 0)]
    public class VoidEventChannelSO : ScriptableObject
    {
        public event UnityAction OnEventRaised;
        
        public void RaiseEvent()
        {
            OnEventRaised?.Invoke();
        }
    }
}