using UnityEngine.Events;
using UnityEngine;

namespace Horror.Gameplay.EventTrigger
{
    [RequireComponent(typeof(Collider))]
    public sealed class EventTrigger : MonoBehaviour
    {
        [SerializeField] private bool _enterOnlyOnce = true;
        [SerializeField] private UnityEvent _enterEvent = new UnityEvent();
        
        [SerializeField] private bool _stayOnlyOnce = true;
        [SerializeField] private UnityEvent _stayEvent = new UnityEvent();
        
        [SerializeField] private bool _exitOnlyOnce = true;
        [SerializeField] private UnityEvent _exitEvent = new UnityEvent();
        
        private bool _canEnter = true;
        private bool _canStay = true;
        private bool _canExit = true;

        private void OnTriggerEnter(Collider other)
        {
            if (_canEnter == false)
            {
                return;
            }

            _enterEvent.Invoke();

            if (_enterOnlyOnce)
            {
                _canEnter = false;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (_canStay == false)
            {
                return;
            }

            _stayEvent.Invoke();

            if (_stayOnlyOnce)
            {
                _canStay = false;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (_canExit == false)
            {
                return;
            }

            _exitEvent.Invoke();

            if (_exitOnlyOnce)
            {
                _canExit = false;
            }
        }
    }
}
