using UnityEngine;

namespace Horror.UI.Screens
{
    public sealed class LoadingScreen : UIScreen
    {
        [Header("Fade")] 
        [SerializeField] private UIFader _uiFader;

        [Header("Loading Bar")] 
        [SerializeField] private LoadingBarController _loadingBarController;

        protected override void OnOpen()
        {
            base.OnOpen();
            
            _uiFader.SetCanvasGroupAlpha(1f);

            _loadingBarController.Initialize();
        }

        protected override void OnClose()
        {
            float endValue = 0f;
            
            _uiFader.Fade(endValue);

            _uiFader.OnFadeCompleted += HandleFadeCompleted;
        }

        private void HandleFadeCompleted()
        {
            gameObject.SetActive(false);
            
            _uiFader.OnFadeCompleted -= HandleFadeCompleted;
        }
    }
}
