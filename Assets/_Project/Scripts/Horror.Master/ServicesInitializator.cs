using Horror.Gameplay.VoiceRecognizer;
using UnityEngine.SceneManagement;
using Horror.ServiceLocator;
using Horror.Events;
using Horror.Inputs;
using UnityEngine;

namespace Horror.Master
{
    public static class ServicesInitializator
    {
        private const string StartupSceneName = "Startup";
        
        // [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void LoadStartupScene()
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            
            if (currentSceneName != StartupSceneName)
            {
                SceneManager.LoadScene(StartupSceneName, LoadSceneMode.Additive);
            }
        }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitializeServices()
        {
            InitializeEventService();

            InitializeInputService();

            InitializeVoiceService();
        }
        
        private static void InitializeEventService()
        {
            IEventService eventService = new EventService();
            
            GameServices.RegisterService(eventService);
        }
        
        private static void InitializeInputService()
        {
            IInputService inputService = new PlayerInputService();
            
            GameServices.RegisterService(inputService);
        }
        
        private static void InitializeVoiceService()
        {
            IVoiceService voiceService = new VoiceService();
            
            GameServices.RegisterService(voiceService);
        }
    }
}
