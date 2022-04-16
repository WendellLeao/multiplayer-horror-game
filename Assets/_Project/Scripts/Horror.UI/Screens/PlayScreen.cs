using UnityEngine.SceneManagement;
using UnityEngine;

namespace Horror.UI.Screens
{
    public sealed class PlayScreen : UIScreen
    {
        [SerializeField] private HoverButton _playButton;
        [SerializeField] private HoverButton _quitButton;

        public void Initialize()
        {
            _playButton.OnButtonClicked += HandlePlayButtonClicked;
            _quitButton.OnButtonClicked += HandleQuitButtonClicked;
        }

        public void Dispose()
        {
            _playButton.OnButtonClicked -= HandlePlayButtonClicked;
            _quitButton.OnButtonClicked -= HandleQuitButtonClicked;
        }

        private void HandlePlayButtonClicked()
        {
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            
            SceneManager.LoadScene(nextSceneIndex);
        }
        
        private void HandleQuitButtonClicked()
        {
            Application.Quit();
        }
    }
}
