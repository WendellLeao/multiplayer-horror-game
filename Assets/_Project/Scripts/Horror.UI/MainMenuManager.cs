using Horror.UI.Screens.MainMenu;
using Horror.ServiceLocator;
using Horror.UI.Screens;
using UnityEngine;

namespace Horror.UI.MainMenu
{
    public sealed class MainMenuManager : MonoBehaviour
    {
        private void Awake()
        {
            IUIService uiService = GameServices.GetService<IUIService>();
            
            UIScreen playScreen = uiService.OpenScreen<PlayScreen>();

            playScreen.Initialize();
        }
    }
}
