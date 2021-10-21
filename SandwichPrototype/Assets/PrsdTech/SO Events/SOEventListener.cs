using System;
using UnityEngine;

namespace PrsdTech.SO.Events
{

    [Serializable]
    public class SOEventListener : ISOEventListener
    {
        [SerializeField] SOEvent eventReference;

        public Action<SOEventArgs> OnEvent;

        public UnityEngine.Object Object => OnEvent.Target as UnityEngine.Object;

        public SOEventListener(SOEvent eventReference, Action<SOEventArgs> OnEvent = null, bool enable = false)
        {
            this.eventReference = eventReference;
            this.OnEvent = OnEvent;
            if (enable)
            {
                Enable();
            }
        }

        public void Enable()
        {
            if (CheckForMissingEvent())
            {
                eventReference.Subscribe(this);
            }
        }

        public void Enable(Action<SOEventArgs> OnEvent)
        {
            this.OnEvent = OnEvent;
            if (CheckForMissingEvent())
            {
                eventReference.Subscribe(this);
            }
        }

        public void Disable()
        {
            if (CheckForMissingEvent())
            {
                eventReference.Unsubscribe(this);
            }
        }

        public void OnEventInvoked(SOEventArgs args = null)
        {
            OnEvent?.Invoke(args);
        }

        bool CheckForMissingEvent()
        {
            if (!eventReference)
            {
                if (OnEvent != null)
                {
                    Debug.LogWarning(Object.name + ": Missing event reference.");
                }
                else
                {
                    Debug.LogWarning("Missing event reference.");
                }
                return false;
            }
            return true;
        }
    }

}
