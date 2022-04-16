using Cinemachine;
using UnityEngine;

namespace Horror.Gameplay
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;
        [SerializeField] private Camera _mainCamera;
        
        public static PlayerCamera Instance { get; private set; }

        public CinemachineVirtualCamera VirtualCamera => _virtualCamera;
        public Camera MainCamera => _mainCamera;

        private void Awake()
        {
            Instance = this;
        }
    }
}
