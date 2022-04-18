using Castle.Core.Internal;
using UnityEngine;

namespace Horror.UI.Screens
{
    public sealed class PlayerNameScreen : UIScreen
    {
        [SerializeField] private PlayerInputField _playerInputField;
        [SerializeField] private HoverButton _continueButton;
        [SerializeField] private HoverButton _backButton;

        [Header("Screens")] 
        [SerializeField] private LobbyScreen _lobbyScreen;
        
        protected override void OnInitialize()
        {
            base.OnInitialize();
         
            _lobbyScreen.Initialize();
        }

        protected override void OnOpen()
        {
            base.OnOpen();
            
            _playerInputField.Initialize();

            string playerName = _playerInputField.PlayerName;
            
            if (!playerName.IsNullOrEmpty())
            {
                return;
            }
            
            _continueButton.IsInteractable = false;
        }

        protected override void OnClose()
        {
            base.OnClose();
            
            _playerInputField.Dispose();
        }

        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();

            _playerInputField.OnSubmitted += HandlePlayerInputFieldSubmitted;
            _continueButton.OnButtonClicked += HandleContinueButtonClicked;
            _backButton.OnButtonClicked += Close;
        }

        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();
            
            _playerInputField.OnSubmitted -= HandlePlayerInputFieldSubmitted;
            _continueButton.OnButtonClicked -= HandleContinueButtonClicked;
            _backButton.OnButtonClicked -= Close;
        }

        private void HandlePlayerInputFieldSubmitted(string playerName)
        {
            if (playerName.IsNullOrEmpty())
            {
                _continueButton.IsInteractable = false;
                
                return;
            }

            _continueButton.IsInteractable = true;
        }

        private void HandleContinueButtonClicked()
        {
            UIService.OpenScreen(_lobbyScreen);
        }
    }
}