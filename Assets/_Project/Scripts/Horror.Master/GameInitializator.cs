using UnityEngine.SceneManagement;
using UnityEngine;

namespace Horror.Master
{
    public class GameInitializator : MonoBehaviour
    {
        private void Awake()
        {
            Application.targetFrameRate = 60;//TODO: UNLOCK FRAME RATE
            
            StartGame();
        }

        private void StartGame()
        {
            Debug.Log("Game started!");
            
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}
