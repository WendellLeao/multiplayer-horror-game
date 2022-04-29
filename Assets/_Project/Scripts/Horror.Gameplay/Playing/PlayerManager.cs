using Horror.Gameplay.Cameras;
using Horror.ServiceLocator;
using Horror.Events;
using UnityEngine;
using Mirror;

namespace Horror.Gameplay.Playing
{
    public sealed class PlayerManager : NetworkBehaviour
    {
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private Transform _spawnPosition;

        private Player _localPlayer;
        
        [Server]
        public void Begin(NetworkConnectionToClient conn)
        {
            ServerHandlePlayerSpawn(conn);
        }

        public void Dispose()
        { }

        public void Stop()
        {
            if (_localPlayer == null)
            {
                return;
            }
            
            _localPlayer.Stop();
        }

        public void Tick(float deltaTime)
        {
            if (_localPlayer == null)
            {
                return;
            }
            
            _localPlayer.Tick(deltaTime);
        }

        [Server]
        private void ServerHandlePlayerSpawn(NetworkConnectionToClient conn)
        {
            GameObject playerObject = CreatePlayer(conn, _spawnPosition);

            FirstPersonCamera firstPersonCamera = CreateAndInitializeCamera(conn, playerObject);
            
            TargetRpcInitializePlayer(conn, playerObject, firstPersonCamera.gameObject);

            RpcDispatchEvent(playerObject);
        }

        [Server]
        private GameObject CreatePlayer(NetworkConnectionToClient conn, Transform spawnPosition)
        {
            GameObject playerObject = Instantiate(_playerPrefab);
            
            playerObject.transform.position = spawnPosition.position;

            NetworkServer.AddPlayerForConnection(conn, playerObject);
            
            return playerObject;
        }

        [TargetRpc]
        private void TargetRpcInitializePlayer(NetworkConnection conn, GameObject playerObject, GameObject firstPersonCameraObject)
        {
            _localPlayer = playerObject.GetComponent<Player>();
                
            FirstPersonCamera firstPersonCamera = firstPersonCameraObject.GetComponent<FirstPersonCamera>();
            
            ICameraService cameraService = GameServices.GetService<ICameraService>();

            _localPlayer.Begin(cameraService, firstPersonCamera);
        }

        [Server]
        private FirstPersonCamera CreateAndInitializeCamera(NetworkConnectionToClient conn, GameObject playerObject)
        {
            ICameraService cameraService = GameServices.GetService<ICameraService>();

            FirstPersonCamera firstPersonCamera = cameraService.CreateFirstPersonCamera();

            GameObject firstPersonCameraObject = firstPersonCamera.gameObject;

            TargetRpcInitializeCamera(conn, firstPersonCameraObject, playerObject);

            return firstPersonCamera;
        }

        [TargetRpc]
        private void TargetRpcInitializeCamera(NetworkConnection conn, GameObject firstPersonCameraObject, GameObject playerObject)
        {
            FirstPersonCamera firstPersonCamera = firstPersonCameraObject.GetComponent<FirstPersonCamera>();

            Player player = playerObject.GetComponent<Player>();

            firstPersonCamera.Initialize(player.CameraTarget);
        }
        
        [ClientRpc]
        private void RpcDispatchEvent(GameObject playerObject)
        {
            Player player = playerObject.GetComponent<Player>();
            
            IEventService eventService = GameServices.GetService<IEventService>();
            
            eventService.DispatchEvent(new PlayerCreatedEvent(player));
        }
    }
}
