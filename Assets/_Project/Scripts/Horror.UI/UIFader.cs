using DG.Tweening;
using UnityEngine;
using System;

namespace Horror.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public sealed class UIFader : MonoBehaviour
    {
        public event Action OnFadeCompleted;
        
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _fadeDuration = 1f;

        public void Fade(float endValue)
        {
            _canvasGroup.DOFade(endValue, _fadeDuration).OnComplete(HandleFadeComplete);
        }

        public void SetCanvasGroupAlpha(float value)
        {
            _canvasGroup.alpha = value;
        }
        
        private void HandleFadeComplete()
        {
            OnFadeCompleted?.Invoke();
        }
    }
}