using Horror.ServiceLocator;
using UnityEngine;

namespace Horror.UI.Screens.MainMenu
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