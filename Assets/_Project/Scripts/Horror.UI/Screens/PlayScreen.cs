using UnityEngine.SceneManagement;
using Horror.ServiceLocator;
using Horror.Audio;
using UnityEngine;

namespace Horror.UI.Screens
{
    public sealed class PlayScreen : UIScreen
    {
        [SerializeField] private HoverButton _playButton;
        [SerializeField] private HoverButton _quitButton;

        public void Initialize()
        {
            IAudioService audioService = GameServices.GetService<IAudioService>();
            
            audioService.PlaySound(Sound.GameTheme, Vector3.zero);
            
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
