using Horror.Gameplay.VoiceRecognizer;
using Horror.ServiceLocator;
using Horror.Networking;
using Horror.Pooling;
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
        private const string UIServicePrefabPath = "GameServices/UIService/UIService";
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitializeServices()
        {
            CheckAndInitializeNetworkService();

            CheckAndInitializePoolingService();
                
            CheckAndInitializeUIService();
            
            CheckAndInitializeEventService();
            
            CheckAndInitializeAudioService();
            
            CheckAndInitializeInputService();
            
            CheckAndInitializeVoiceService();
        }

        private static void CheckAndInitializeNetworkService()
        {
            INetworkService networkService = GameServices.GetService<INetworkService>();

            if (networkService != null)
            {
                Debug.LogWarning($"{networkService} is already registered");
                
                return;
            }
            
            GameObject networkServicePrefab = Resources.Load(NetworkServicePrefabPath) as GameObject;
                
            Object.Instantiate(networkServicePrefab);
        }
        
        private static void CheckAndInitializePoolingService()
        {
            IPoolingService poolingService = GameServices.GetService<IPoolingService>();

            if (poolingService != null)
            {
                Debug.LogWarning($"{poolingService} is already registered");
                
                return;
            }
            
            GameObject poolingServicePrefab = Resources.Load(PoolingServicePrefabPath) as GameObject;
                
            Object.Instantiate(poolingServicePrefab);
        }

        private static void CheckAndInitializeUIService()
        {
            IUIService uiService = GameServices.GetService<IUIService>();

            if (uiService != null)
            {
                Debug.LogWarning($"{uiService} is already registered");
                
                return;
            }
            
            GameObject uiServicePrefab = Resources.Load(UIServicePrefabPath) as GameObject;
                
            Object.Instantiate(uiServicePrefab);
        }

        private static void CheckAndInitializeEventService()
        {
            IEventService eventService = GameServices.GetService<IEventService>();

            if (eventService != null)
            {
                Debug.LogWarning($"{eventService} is already registered");
                
                return;
            }
            
            IEventService newEventService = new EventService();
            
            GameServices.RegisterService(newEventService);
        }

        private static void CheckAndInitializeAudioService()
        {
            IAudioService audioService = GameServices.GetService<IAudioService>();

            if (audioService != null)
            {
                Debug.LogWarning($"{audioService} is already registered");
                
                return;
            }
            
            IAudioService newAudioService = new AudioService();
            
            GameServices.RegisterService(newAudioService);
        }
        
        private static void CheckAndInitializeInputService()
        {
            IInputService inputService = GameServices.GetService<IInputService>();

            if (inputService != null)
            {
                Debug.LogWarning($"{inputService} is already registered");
                
                return;
            }
            
            IInputService newInputService = new PlayerInputService();
            
            GameServices.RegisterService(newInputService);
        }
        
        private static void CheckAndInitializeVoiceService()
        {
            IVoiceService voiceService = GameServices.GetService<IVoiceService>();

            if (voiceService != null)
            {
                Debug.LogWarning($"{voiceService} is already registered");
                
                return;
            }
            
            IVoiceService newVoiceService = new VoiceService();
            
            GameServices.RegisterService(newVoiceService);
        }
    }
}
