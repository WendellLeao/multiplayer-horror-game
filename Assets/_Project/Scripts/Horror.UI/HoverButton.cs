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
            foreach (HighlightableText highlightableText in _highlightableTexts)
            {
                highlightableText.EnableGlow();
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            foreach (HighlightableText highlightableText in _highlightableTexts)
            {
                highlightableText.DisableGlow();
            }
        }
        
        public void Invoke()
        {
            _button.onClick.Invoke();
        }
    }
}