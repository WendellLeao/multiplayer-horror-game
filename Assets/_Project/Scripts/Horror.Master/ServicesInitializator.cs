using Horror.Gameplay.VoiceRecognizer;
using UnityEngine.SceneManagement;
using Horror.ServiceLocator;
using Horror.Events;
using Horror.Inputs;
using Horror.Audio;
using Horror.Pooling;
using Horror.UI;
using UnityEngine;

namespace Horror.Master
{
    public static class ServicesInitializator
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitializeServices()
        {
            InitializeUIService();
            
            InitializeEventService();
            
            InitializeAudioService();
            
            InitializeInputService();
            
            InitializeVoiceService();
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
