using UnityEngine.SceneManagement;
using Horror.ServiceLocator;
using Horror.Networking;
using UnityEngine;

namespace Horror.UI.Screens
{
    public sealed class LobbyScreen : UIScreen
    {
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
            
            networkService.StartHost();

            LoadNextScene();
        }

        private void HandleBackButtonClicked()
        {
            UIService.CloseTopScreen();
        }

        private void LoadNextScene()
        {
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}