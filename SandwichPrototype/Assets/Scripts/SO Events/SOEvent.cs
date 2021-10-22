using System.Collections.Generic;
using UnityEngine;

namespace PrsdTech.SO.Events
{

    public interface ISOEventListener
    {
        Object Object { get; }
        void OnEventInvoked(SOEventArgs args = null);
    }

    public class SOEventArgs { }

    [CreateAssetMenu(menuName = "PrsdTech/ScriptableObjects/SOEvent", order = 380)]
    public class SOEvent : ScriptableObject
    {
        List<ISOEventListener> listeners;

        public List<ISOEventListener> GetListeners() => listeners;

        void OnDisable()
        {
            if (listeners != null)
            {
                listeners.Clear();
                listeners = null;
            }
        }

        public void Invoke(SOEventArgs args = null)
        {
            if (listeners != null)
            {
                for (int i = listeners.Count - 1; i >= 0; i--)
                {
                    listeners[i].OnEventInvoked(args);
                }
            }
        }

        public void Subscribe(ISOEventListener l)
        {
            if (listeners == null) { listeners = new List<ISOEventListener>(); }
            listeners.Add(l);
        }

        public void Unsubscribe(ISOEventListener l)
        {
            if (listeners != null)
            {
                listeners.Remove(l);
            }
        }
    }

}
