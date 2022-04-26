using Horror.UI.Screens.MainMenu;
using Horror.ServiceLocator;
using Horror.UI.Screens;
using Horror.Audio;
using UnityEngine;

namespace Horror.UI.MainMenu
{
    public sealed class MainMenuManager : MonoBehaviour
    {
        private IUIService _uiService;
        private UIScreen _playScreen;

        private void Awake()
        {
            PlayGameTheme();
            
            _uiService = GameServices.GetService<IUIService>();

            _playScreen = _uiService.GetRegisteredScreen<PlayScreen>();
            
            if (_uiService.CurrentOpenedScreen == null)
            {
                _uiService.OpenScreen(_playScreen);

                return;
            }
            
            UIScreen loadingScreen = _uiService.CurrentOpenedScreen;
                
            loadingScreen.OnClosed += HandleLoadingScreenClosed;
        }

        private void HandleLoadingScreenClosed(UIScreen uiScreen)
        {
            _uiService.OpenScreen(_playScreen);
            
            uiScreen.OnClosed -= HandleLoadingScreenClosed;
        }

        private void PlayGameTheme()
        {
            IAudioService audioService = GameServices.GetService<IAudioService>();
            
            audioService.PlaySound(Sound.GameTheme, Vector3.zero);
        }
    }
}
