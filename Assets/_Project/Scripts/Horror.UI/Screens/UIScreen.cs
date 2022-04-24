using Horror.ServiceLocator;
using UnityEngine;
using System;

namespace Horror.UI.Screens
{
    public abstract class UIScreen : MonoBehaviour
    {
        public event Action<UIScreen> OnClosed;
        
        private IUIService _uiService;
        private bool _isOpen;

        public bool IsOpen => _isOpen;
        protected IUIService UIService => _uiService;

        public void Initialize()
        {
            OnInitialize();
        }

        public void Close()
        {
            UnsubscribeEvents();
            
            OnClose();

            _isOpen = false;
            
            OnClosed?.Invoke(this);
        }

        protected virtual void SubscribeEvents()
        {}
        
        protected virtual void UnsubscribeEvents()
        {}

        protected virtual void OnInitialize()
        {}
        
        protected virtual void OnOpen()
        {}

        protected virtual void OnClose()
        {
            gameObject.SetActive(false);
        }
        
        protected virtual void OnDestroy()
        {
            _uiService.UnregisterScreen(this);
        }
        
        private void Awake()
        {
            _uiService = GameServices.GetService<IUIService>();
            
            _uiService.RegisterScreen(this);
            
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            SubscribeEvents();
            
            OnOpen();

            _isOpen = true;
        }
    }
}
