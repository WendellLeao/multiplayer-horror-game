using Horror.ServiceLocator;
using UnityEngine;

namespace Horror.UI.Screens
{
    public abstract class UIScreen : MonoBehaviour
    {
        [SerializeField] private bool _autoInitialize;
        
        private IUIService _uiService;

        protected IUIService UIService => _uiService;

        public void Initialize()
        {
            _uiService = GameServices.GetService<IUIService>();
            
            OnInitialize();
        }
        
        protected virtual void OnEnable()
        {
            SubscribeEvents();
        }

        protected virtual void OnDisable()
        {
            UnsubscribeEvents();
        }
        
        protected virtual void SubscribeEvents()
        {}
        
        protected virtual void UnsubscribeEvents()
        {}
        
        protected virtual void OnInitialize()
        {}
        
        private void Awake()
        {
            if (!_autoInitialize)
            {
                return;
            }
            
            Initialize();
        }
    }
}