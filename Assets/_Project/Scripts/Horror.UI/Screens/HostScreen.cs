using UnityEngine.SceneManagement;
using UnityEngine;

namespace Horror.UI.Screens
{
    public sealed class HostScreen : UIScreen
    {
        [SerializeField] private HoverButton _startGameButton;
        [SerializeField] private HoverButton _backButton;

        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();
            
            _startGameButton.OnButtonClicked += HandleStartGameButtonClicked;
            _backButton.OnButtonClicked += HandleBackButtonClicked;
        }

        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();
            
            _startGameButton.OnButtonClicked -= HandleStartGameButtonClicked;
            _backButton.OnButtonClicked -= HandleBackButtonClicked;
        }

        private void HandleStartGameButtonClicked()
        {
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            
            SceneManager.LoadScene(nextSceneIndex);
        }

        private void HandleBackButtonClicked()
        {
            UIService.CloseTopScreen();
        }
    }
}