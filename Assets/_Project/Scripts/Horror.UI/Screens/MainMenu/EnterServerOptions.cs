using Horror.ServiceLocator;
using Horror.Networking;
using UnityEngine;

namespace Horror.UI.Screens.MainMenu
{
    public sealed class EnterServerOptions : UIScreen
    {
        [Header("Player Name")]
        [SerializeField] private PlayerInputField _playerInputField;
        
        [Header("UI")]
        [SerializeField] private HoverButton _joinButton;
        [SerializeField] private HoverButton _hostButton;
        [SerializeField] private HoverButton _backButton;
        
        private UIScreen _joinScreen;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            InitializePlayerInputField();
        }
        
        protected override void OnOpen()
        {
            base.OnOpen();
            
            _joinScreen = UIService.GetRegisteredScreen<JoinScreen>();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            _playerInputField.Dispose();
        }

        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();

            _playerInputField.OnSubmitted += CheckPlayerNameSubmission;
            _joinButton.OnButtonClicked += HandleJoinButtonClicked;
            _hostButton.OnButtonClicked += HandleHostButtonClicked;
            _backButton.OnButtonClicked += HandleBackButtonClicked;
        }

        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();
            
            _playerInputField.OnSubmitted -= CheckPlayerNameSubmission;
            _joinButton.OnButtonClicked -= HandleJoinButtonClicked;
            _hostButton.OnButtonClicked -= HandleHostButtonClicked;
            _backButton.OnButtonClicked -= HandleBackButtonClicked;
        }
        
        private void InitializePlayerInputField()
        {
            _playerInputField.Initialize();

            string playerName = _playerInputField.PlayerName;

            CheckPlayerNameSubmission(playerName);
        }
        
        private void CheckPlayerNameSubmission(string playerName)
        {
            if (string.IsNullOrEmpty(playerName))
            {
                _joinButton.SetInteractable(false);
                _hostButton.SetInteractable(false);
                
                return;
            }

            _joinButton.SetInteractable(true);
            _hostButton.SetInteractable(true);
        }

        private void HandleJoinButtonClicked()
        {
            UIService.OpenScreen(_joinScreen);
        }

        private void HandleHostButtonClicked()
        {
            INetworkService networkService = GameServices.GetService<INetworkService>();
            
            networkService.StartHost();
            
            _hostButton.SetInteractable(false);

            UIService.OpenScreen<LoadingScreen>();
        }

        private void HandleBackButtonClicked()
        {
            UIService.CloseTopScreen();
        }
    }
}