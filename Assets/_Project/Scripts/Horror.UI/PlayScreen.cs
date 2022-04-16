using UnityEngine.SceneManagement;
using UnityEngine;

namespace Horror.UI
{
    public class PlayScreen : MonoBehaviour
    {
        [SerializeField] private HoverButton _playButton;
        [SerializeField] private HoverButton _quitButton;

        private void OnEnable()//TODO: ADJUST THIS
        {
            _playButton.OnButtonClicked += HandlePlayButtonClicked;
            _quitButton.OnButtonClicked += HandleQuitButtonClicked;
        }
        
        private void OnDisable()//TODO: ADJUST THIS
        {
            _playButton.OnButtonClicked -= HandlePlayButtonClicked;
            _quitButton.OnButtonClicked -= HandleQuitButtonClicked;
        }

        private void HandlePlayButtonClicked()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        
        private void HandleQuitButtonClicked()
        {
            Application.Quit();
        }
    }
}
