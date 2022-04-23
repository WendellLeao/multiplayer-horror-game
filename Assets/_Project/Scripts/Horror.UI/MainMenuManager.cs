using Horror.UI.Screens.MainMenu;
using Horror.ServiceLocator;
using UnityEngine;

namespace Horror.UI.MainMenu
{
    public sealed class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private PlayScreen _playScreen;

        private void Awake()
        {
            _playScreen.Initialize();

            IUIService uiService = GameServices.GetService<IUIService>();
            
            uiService.OpenScreen(_playScreen);
        }
    }
}