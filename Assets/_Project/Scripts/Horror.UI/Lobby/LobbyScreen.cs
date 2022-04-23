using UnityEngine.Events;
using Horror.UI.Screens;
using UnityEngine;

namespace Horror.UI.Lobby
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
        
        [Header("UI")] 
        [SerializeField] private GameObject _hostButtonsGroup;
        [SerializeField] private GameObject _clientButtonsGroup;
        [SerializeField] private HoverButton _playHoverButton;
        [SerializeField] private HoverButton _readyHoverButton;

        public void SetPlayButtonInteractable(bool isInteractable)
        {
            _playHoverButton.SetInteractable(isInteractable);
        }
        
        public void ActiveHostButtonsGroup()
        {
            _hostButtonsGroup.SetActive(true);
            _clientButtonsGroup.SetActive(false);
        }
        
        public void ActiveClientButtonsGroup()
        {
            _clientButtonsGroup.SetActive(true);
            _hostButtonsGroup.SetActive(false);
        }

        public void SetReadyButtonLabelText(string text)
        {
            _readyHoverButton.SetLabelText(text);
        }
    }
}