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
        private const string AudioServicePrefabPath = "GameServices/AudioService/AudioService";
        private const string UIServicePrefabPath = "GameServices/UIService/UIService";
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitializeServices()
        {
            if (GameServices.GetService<INetworkService>() == null)
            {
                CheckAndInitializeNetworkService();
            }

            if (GameServices.GetService<IPoolingService>() == null)
            {
                CheckAndInitializePoolingService();
            }

            if (GameServices.GetService<IAudioService>() == null)
            {
                CheckAndInitializeAudioService();
            }

            if (GameServices.GetService<IUIService>() == null)
            {
                CheckAndInitializeUIService();
            }

            if (GameServices.GetService<IEventService>() == null)
            {
                CheckAndInitializeEventService();
            }

            if (GameServices.GetService<IInputService>() == null)
            {
                CheckAndInitializeInputService();
            }

            if (GameServices.GetService<IVoiceService>() == null)
            {
                CheckAndInitializeVoiceService();
            }
        }

        private static void CheckAndInitializeNetworkService()
        {
            GameObject networkServicePrefab = Resources.Load(NetworkServicePrefabPath) as GameObject;
                
            Object.Instantiate(networkServicePrefab);
        }
        
        private static void CheckAndInitializePoolingService()
        {
            GameObject poolingServicePrefab = Resources.Load(PoolingServicePrefabPath) as GameObject;
                
            Object.Instantiate(poolingServicePrefab);
        }
        
        private static void CheckAndInitializeAudioService()
        {
            GameObject audioServicePrefab = Resources.Load(AudioServicePrefabPath) as GameObject;
                
            Object.Instantiate(audioServicePrefab);
        }

        private static void CheckAndInitializeUIService()
        {
            GameObject uiServicePrefab = Resources.Load(UIServicePrefabPath) as GameObject;
                
            Object.Instantiate(uiServicePrefab);
        }
        
        private static void CheckAndInitializeEventService()
        {
            IEventService newEventService = new EventService();
            
            GameServices.RegisterService(newEventService);
        }

        private static void CheckAndInitializeInputService()
        {
            IInputService newInputService = new PlayerInputService();
            
            GameServices.RegisterService(newInputService);
        }
        
        private static void CheckAndInitializeVoiceService()
        {
            IVoiceService newVoiceService = new VoiceService();
            
            GameServices.RegisterService(newVoiceService);
        }
    }
}
