using Horror.ServiceLocator;
using UnityEngine;

namespace Horror.UI.Screens
{
    public abstract class UIScreen : MonoBehaviour
    {
        private IUIService _uiService;

        protected IUIService UIService => _uiService;

        public void Initialize()
        {
            _uiService = GameServices.GetService<IUIService>();
            
            OnInitialize();
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
        {}

        protected void Close()
        {
            UIService.CloseTopScreen();
        }
        
        private void OnEnable()
        {
            SubscribeEvents();
            
            OnOpen();
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
            
            OnClose();
        }
    }
}