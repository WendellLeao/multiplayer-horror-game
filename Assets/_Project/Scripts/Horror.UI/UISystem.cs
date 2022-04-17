using Horror.ServiceLocator;
using Horror.UI.Screens;
using Horror.Audio;
using Horror.Pooling;
using UnityEngine;

namespace Horror.UI
{
    public sealed class UISystem : MonoBehaviour, IUIService
    {
        [SerializeField] private PlayScreen _playScreen;

        public void OpenScreen<T>() where T : UIScreen
        { }

        public void CloseScreen<T>() where T : UIScreen
        { }

        private void Awake()
        {
            IPoolingService poolingService = GameServices.GetService<IPoolingService>();
            
            poolingService.Begin();
            
            IAudioService audioService = GameServices.GetService<IAudioService>();
            
            audioService.PlaySound(Sound.GameTheme, Vector3.zero);
            
            _playScreen.Initialize();
        }

        private void OnDestroy()
        {
            _playScreen.Dispose();
        }
    }
}
