using UnityEngine.Events;
using UnityEngine;

namespace Horror.UI.Screens.Lobby
{
    public sealed class LobbyScreen : UIScreen
    {
        public event UnityAction OnPlayButtonClicked
        {
            add => _playButton.OnButtonClicked += value;
            remove => _playButton.OnButtonClicked -= value;
        }
        
        public event UnityAction OnReadyButtonClicked
        {
            add => _readyButton.OnButtonClicked += value;
            remove => _readyButton.OnButtonClicked -= value;
        }
        
        public event UnityAction OnBackButtonClicked
        {
            add => _backButton.OnButtonClicked += value;
            remove => _backButton.OnButtonClicked -= value;
        }

        [SerializeField] private UIFader _uiFader;
        [SerializeField] private HoverButton _playButton;
        [SerializeField] private HoverButton _readyButton;
        [SerializeField] private HoverButton _backButton;

        public void SetPlayButtonInteractable(bool isInteractable)
        {
            _playButton.SetInteractable(isInteractable);
        }
        
        public void ActiveHostButtonsGroup()
        {
            _playButton.gameObject.SetActive(true);
            _readyButton.gameObject.SetActive(false);
        }
        
        public void ActiveClientButtonsGroup()
        {
            _readyButton.gameObject.SetActive(true);
            _playButton.gameObject.SetActive(false);
        }

        public void SetReadyButtonLabelText(string text)
        {
            _readyButton.SetLabelText(text);
        }
        
        protected override void OnOpen()
        {
            base.OnOpen();
            
            float endValue = 1f;

            _uiFader.Fade(endValue);
        }
    }
}