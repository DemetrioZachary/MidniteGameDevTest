using UnityEngine;

namespace PrsdTech.SO.Events
{

    public class SOEventListenerComponent : MonoBehaviour, ISOEventListener
    {
        [SerializeField] SOEvent eventReference;
        [SerializeField] UnityEngine.Events.UnityEvent OnEvent;

        public Object Object => gameObject;

        void OnEnable()
        {
            eventReference.Subscribe(this);
        }

        void OnDisable()
        {
            eventReference.Unsubscribe(this);
        }

        public void OnEventInvoked(SOEventArgs args)
        {
            OnEvent.Invoke();
        }
    }

}
