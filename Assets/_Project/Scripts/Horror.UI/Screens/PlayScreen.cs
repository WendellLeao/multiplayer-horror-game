using Horror.ServiceLocator;
using Horror.Audio;
using UnityEngine;

namespace Horror.UI.Screens
{
    public sealed class PlayScreen : UIScreen
    {
        [Header("Buttons")]
        [SerializeField] private HoverButton _playButton;
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
            
            _playButton.OnButtonClicked += HandlePlayButtonClicked;
            _quitButton.OnButtonClicked += HandleQuitButtonClicked;
        }

        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();
            
            _playButton.OnButtonClicked -= HandlePlayButtonClicked;
            _quitButton.OnButtonClicked -= HandleQuitButtonClicked;
        }

        private void HandlePlayButtonClicked()
        {
            UIService.OpenScreen(_playerNameScreen);
        }
        
        private void HandleQuitButtonClicked()
        {
            Application.Quit();
        }
    }
}
