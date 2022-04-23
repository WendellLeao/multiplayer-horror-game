using Horror.ServiceLocator;
using Horror.Audio;
using Horror.Networking;
using UnityEngine;

namespace Horror.UI.Screens
{
    public sealed class PlayScreen : UIScreen
    {
        [Header("Buttons")]
        [SerializeField] private HoverButton _soloButton;
        [SerializeField] private HoverButton _multiplayerButton;
        [SerializeField] private HoverButton _quitButton;
        
        [Header("Screens")]
        [SerializeField] private PlayerNameScreen _playerNameScreen;

        protected override void OnInitialize()
        {
            base.OnInitialize();
            
            IAudioService audioService = GameServices.GetService<IAudioService>();
            
            audioService.PlaySound(Sound.GameTheme, Vector3.zero);
            
            _playerNameScreen.Initialize();
        }

        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();

            _multiplayerButton.OnButtonClicked += HandleMultiplayerButtonClicked;
            _soloButton.OnButtonClicked += HandleSoloButtonClicked;
            _quitButton.OnButtonClicked += HandleQuitButtonClicked;
        }

        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();
            
            _multiplayerButton.OnButtonClicked -= HandleMultiplayerButtonClicked;
            _soloButton.OnButtonClicked -= HandleSoloButtonClicked;
            _quitButton.OnButtonClicked -= HandleQuitButtonClicked;
        }

        private void HandleMultiplayerButtonClicked()
        {
            UIService.OpenScreen(_playerNameScreen);
        }

        private void HandleSoloButtonClicked()
        {
            INetworkService networkService = GameServices.GetService<INetworkService>();
            
            networkService.StartHost();
        }
        
        private void HandleQuitButtonClicked()
        {
            Application.Quit();
        }
    }
}
