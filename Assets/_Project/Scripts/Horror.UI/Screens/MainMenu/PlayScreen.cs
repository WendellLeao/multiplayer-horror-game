using Horror.ServiceLocator;
using Horror.Networking;
using Horror.Audio;
using UnityEngine;

namespace Horror.UI.Screens.MainMenu
{
    public sealed class PlayScreen : UIScreen
    {
        [Header("Buttons")]
        [SerializeField] private HoverButton _soloButton;
        [SerializeField] private HoverButton _multiplayerButton;
        [SerializeField] private HoverButton _quitButton;
        
        private UIScreen _enterServerOptions;

        protected override void OnInitialize()
        {
            base.OnInitialize();
            
            IAudioService audioService = GameServices.GetService<IAudioService>();
            
            audioService.PlaySound(Sound.GameTheme, Vector3.zero);

            _enterServerOptions = UIService.GetRegisteredScreen<EnterServerOptions>();
            
            _enterServerOptions.Initialize();
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
            UIService.OpenScreen(_enterServerOptions);
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
