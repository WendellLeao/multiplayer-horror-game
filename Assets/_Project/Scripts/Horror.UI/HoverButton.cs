using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;

namespace Horror.UI
{
    public sealed class HoverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public event UnityAction OnButtonClicked
        {
            add => _button.onClick.AddListener(value);
            remove => _button.onClick.RemoveListener(value);
        }
        
        [SerializeField] private Button _button = default;
        [SerializeField] private HighlightableText[] _highlightableTexts;

        public void OnPointerEnter(PointerEventData eventData)
        {
            EnableGlow();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            DisableGlow();
        }
        
        public void Invoke()
        {
            _button.onClick.Invoke();
        }

        private void OnDisable()
        {
            DisableGlow();
        }

        private void EnableGlow()
        {
            foreach (HighlightableText highlightableText in _highlightableTexts)
            {
                highlightableText.EnableGlow();
            }
        }

        private void DisableGlow()
        {
            foreach (HighlightableText highlightableText in _highlightableTexts)
            {
                highlightableText.DisableGlow();
            }
        }
    }
}