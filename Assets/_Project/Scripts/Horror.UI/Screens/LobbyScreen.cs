using Horror.ServiceLocator;
using Horror.Networking;
using UnityEngine;
using Mirror;

namespace Horror.UI.Screens
{
    public sealed class LobbyScreen : UIScreen
    {
        [Scene]
        [SerializeField] private string _gameSceneName;
        
        [Header("UI")]
        [SerializeField] private HoverButton _joinButton;
        [SerializeField] private HoverButton _hostButton;
        [SerializeField] private HoverButton _backButton;

        [Header("Screens")] 
        [SerializeField] private JoinScreen _joinScreen;

        protected override void OnInitialize()
        {
            base.OnInitialize();
            
            _joinScreen.Initialize();
        }

        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();

            _joinButton.OnButtonClicked += HandleJoinButtonClicked;
            _hostButton.OnButtonClicked += HandleHostButtonClicked;
            _backButton.OnButtonClicked += HandleBackButtonClicked;
        }

        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();
            
            _joinButton.OnButtonClicked -= HandleJoinButtonClicked;
            _hostButton.OnButtonClicked -= HandleHostButtonClicked;
            _backButton.OnButtonClicked -= HandleBackButtonClicked;
        }

        private void HandleJoinButtonClicked()
        {
            UIService.OpenScreen(_joinScreen);
        }

        private void HandleHostButtonClicked()
        {
            INetworkService networkService = GameServices.GetService<INetworkService>();
            
            networkService.ServerChangeScene(_gameSceneName);

            networkService.StartHost();
        }

        private void HandleBackButtonClicked()
        {
            UIService.CloseTopScreen();
        }
    }
}