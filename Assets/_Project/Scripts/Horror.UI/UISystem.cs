using Horror.UI.Screens;
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
            _playScreen.Initialize();
        }

        private void OnDestroy()
        {
            _playScreen.Dispose();
        }
    }
}
