using Horror.UI.Screens.MainMenu;
using Horror.ServiceLocator;
using Horror.UI.Screens;
using Horror.Audio;
using UnityEngine;

namespace Horror.UI.MainMenu
{
    public sealed class MainMenuManager : MonoBehaviour
    {
        private void Awake()
        {
            PlayGameTheme();
            
            OpenPlayScreen();
        }

        private static void OpenPlayScreen()
        {
            IUIService uiService = GameServices.GetService<IUIService>();

            UIScreen playScreen = uiService.OpenScreen<PlayScreen>();

            playScreen.Initialize();
        }

        private void PlayGameTheme()
        {
            IAudioService audioService = GameServices.GetService<IAudioService>();
            
            audioService.PlaySound(Sound.GameTheme, Vector3.zero);
        }
    }
}
