using DG.Tweening;
using UnityEngine;

namespace Horror.UI.Screens
{
    public sealed class LoadingScreen : UIScreen
    {
        [Header("Canvas")]
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _fadeDuration;

        [Header("Loading Bar")] 
        [SerializeField] private LoadingBarController _loadingBarController;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            _canvasGroup.alpha = 0f;
        }

        protected override void OnOpen()
        {
            base.OnOpen();
            
            _canvasGroup.alpha = 1f;

            _loadingBarController.Initialize();
        }

        protected override void OnClose()
        {
            _canvasGroup.DOFade(0f, _fadeDuration).OnComplete(HandleFadeComplete);
        }

        private void HandleFadeComplete()
        {
            gameObject.SetActive(false);
        }
    }
}
