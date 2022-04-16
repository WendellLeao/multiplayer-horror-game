using Horror.ServiceLocator;
using UnityEngine;
using Mirror;

namespace Horror.Gameplay.Cameras
{
    public sealed class CameraService : NetworkBehaviour, ICameraService
    {
        [SerializeField] private GameObject _firstPersonCameraPrefab;

        private FirstPersonCamera _localFirstPersonCamera;
        private Transform _itemContainer;

        public Camera MainCamera => Camera.main;
        public Transform ItemContainer => _itemContainer;
        public FirstPersonCamera LocalFirstPersonCamera => _localFirstPersonCamera;

        public FirstPersonCamera CreateFirstPersonCamera()
        {
            GameObject firstPersonCameraObject = Instantiate(_firstPersonCameraPrefab);

            NetworkServer.Spawn(firstPersonCameraObject);

            FirstPersonCamera firstPersonCamera = firstPersonCameraObject.GetComponent<FirstPersonCamera>();

            if (isLocalPlayer)
            {
                _localFirstPersonCamera = firstPersonCamera;
            }
            
            return firstPersonCamera;
        }
        
        private void Awake()
        {
            GameServices.RegisterService<ICameraService>(this);
            
            MainCamera mainCamera = MainCamera.GetComponent<MainCamera>();

            _itemContainer = mainCamera.ItemContainer;
        }

        private void OnDestroy()
        {
            GameServices.DeregisterService<ICameraService>();
        }
    }
}