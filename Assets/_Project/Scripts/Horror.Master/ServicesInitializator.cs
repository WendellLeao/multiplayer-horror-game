using Horror.Gameplay.VoiceRecognizer;
using Horror.ServiceLocator;
using Horror.Events;
using Horror.Inputs;
using Horror.Audio;
using UnityEngine;
using Horror.UI;

namespace Horror.Master
{
    public static class ServicesInitializator
    {
        private const string NetworkServicePrefabPath = "GameServices/NetworkService/NetworkService";
        private const string PoolingServicePrefabPath = "GameServices/PoolingService/PoolingService";

        private static bool _hasInitialized;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitializeServices()
        {
            if (_hasInitialized)
            {
                return;
            }
            
            InitializeNetworkService();

            InitializePoolingService();
                
            InitializeUIService();
            
            InitializeEventService();
            
            InitializeAudioService();
            
            InitializeInputService();
            
            InitializeVoiceService();

            _hasInitialized = true;
        }

        private static void InitializeNetworkService()
        {
            GameObject networkServicePrefab = Resources.Load(NetworkServicePrefabPath) as GameObject;
                
            Object.Instantiate(networkServicePrefab);
        }
        
        private static void InitializePoolingService()
        {
            GameObject poolingServicePrefab = Resources.Load(PoolingServicePrefabPath) as GameObject;
                
            Object.Instantiate(poolingServicePrefab);
        }

        private static void InitializeUIService()
        {
            IUIService uiService = new UIService();
            
            GameServices.RegisterService(uiService);
        }

        private static void InitializeEventService()
        {
            IEventService eventService = new EventService();
            
            GameServices.RegisterService(eventService);
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
    }
}
