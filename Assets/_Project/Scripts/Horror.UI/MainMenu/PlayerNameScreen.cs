using UnityEngine;

namespace Horror.UI.Screens
{
    public sealed class PlayerNameScreen : UIScreen
    {
        [SerializeField] private PlayerInputField _playerInputField;
        [SerializeField] private HoverButton _continueButton;
        [SerializeField] private HoverButton _backButton;

        [Header("Screens")] 
        [SerializeField] private EnterServerOptions _enterServerOptions;
        
        protected override void OnInitialize()
        {
            base.OnInitialize();
         
            _enterServerOptions.Initialize();
        }

        protected override void OnOpen()
        {
            base.OnOpen();
            
            _playerInputField.Initialize();

            string playerName = _playerInputField.PlayerName;
            
            if (string.IsNullOrEmpty(playerName))
            {
                _continueButton.SetInteractable(false);
            }
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
            if (string.IsNullOrEmpty(playerName))
            {
                _continueButton.SetInteractable(false);
                
                return;
            }

            _continueButton.SetInteractable(true);
        }

        private void HandleContinueButtonClicked()
        {
            UIService.OpenScreen(_enterServerOptions);
        }
    }
}