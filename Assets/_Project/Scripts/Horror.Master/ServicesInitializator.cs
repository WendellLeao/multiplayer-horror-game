using Horror.Gameplay.VoiceRecognizer;
using UnityEngine.SceneManagement;
using Horror.ServiceLocator;
using Horror.Pooling;
using Horror.Events;
using Horror.Inputs;
using Horror.Audio;
using UnityEngine;

namespace Horror.Master
{
    public static class ServicesInitializator
    {
        private static bool _hasInitializedServices;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitializeServices()
        {
            if (_hasInitializedServices)
            {
                return;
            }
            
            if (SceneManager.GetActiveScene().name != "Startup")
            {
                SceneManager.LoadScene("Startup");
                
                return;
            }
            
            InitializeEventService();

            InitializePoolingService();

            InitializeAudioService();

            InitializeInputService();

            InitializeVoiceService();

            LoadNextScene();
            
            _hasInitializedServices = true;
        }
        
        private static void InitializeEventService()
        {
            IEventService eventService = new EventService();
            
            GameServices.RegisterService(eventService);
        }

        private static void InitializePoolingService()
        {
            // IPoolingService poolingService = new PoolingService();
            //
            // GameServices.RegisterService(poolingService);
        }

        private static void InitializeAudioService()
        {
            IAudioService audioService = new AudioService();
            
            GameServices.RegisterService(audioService);
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

        private static void LoadNextScene()
        {
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}
