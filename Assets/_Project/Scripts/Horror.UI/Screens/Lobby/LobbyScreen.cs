using UnityEngine.Events;
using UnityEngine;

namespace Horror.UI.Screens.Lobby
{
    public sealed class LobbyScreen : UIScreen
    {
        public event UnityAction OnPlayButtonClicked
        {
            add => _playHoverButton.OnButtonClicked += value;
            remove => _playHoverButton.OnButtonClicked -= value;
        }
        
        public event UnityAction OnReadyButtonClicked
        {
            add => _readyHoverButton.OnButtonClicked += value;
            remove => _readyHoverButton.OnButtonClicked -= value;
        }

        [SerializeField] private UIFader _uiFader;
        [SerializeField] private GameObject _hostHintPanel;
        [SerializeField] private GameObject _clientHintPanel;
        [SerializeField] private HoverButton _playHoverButton;
        [SerializeField] private HoverButton _readyHoverButton;

        public void SetPlayButtonInteractable(bool isInteractable)
        {
            _playHoverButton.SetInteractable(isInteractable);
        }
        
        public void ActiveHostButtonsGroup()
        {
            _hostHintPanel.SetActive(true);
            _clientHintPanel.SetActive(false);
        }
        
        public void ActiveClientButtonsGroup()
        {
            _clientHintPanel.SetActive(true);
            _hostHintPanel.SetActive(false);
        }

        public void SetReadyButtonLabelText(string text)
        {
            _readyHoverButton.SetLabelText(text);
        }
        
        protected override void OnOpen()
        {
            base.OnOpen();
            
            float endValue = 1f;

            _uiFader.Fade(endValue);
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