using Horror.Gameplay.VoiceRecognizer;
using Horror.Networking.Events;
using Horror.Gameplay.Playing;
using Horror.Gameplay.Items;
using Horror.ServiceLocator;
using Horror.Events;
using UnityEngine;
using Mirror;

namespace Horror.Gameplay
{
    public sealed class GameplaySystem : MonoBehaviour
    {
        [Header("Managers")]
        [SerializeField] private PlayerManager _playerManager;
        [SerializeField] private CursorManager _cursorManager;
        [SerializeField] private ItemManager _itemManager;
        [SerializeField] private VoiceListener _voiceListener;//

        private IEventService _eventService;

        private void Awake()
        {
            _playerManager.Initialize();
            _cursorManager.Initialize();
            _itemManager.Initialize();

            _eventService = GameServices.GetService<IEventService>();
            
            _eventService.AddEventListener<ServerDisconnectedEvent>(ServerHandleServerDisconnected);
            _eventService.AddEventListener<ServerReadiedEvent>(ServerHandleServerReadied);
            _eventService.AddEventListener<ServerStoppedEvent>(ServerHandleServerStopped);
            _eventService.AddEventListener<ClientConnectedEvent>(ClientHandleClientConnected);
            _eventService.AddEventListener<ClientStartedEvent>(ClientHandleClientStarted);
            _eventService.AddEventListener<ClientStoppedEvent>(ClientHandleClientStopped);
        }

        private void OnDestroy()
        {
            _playerManager.Dispose();
            _cursorManager.Dispose();
            _itemManager.Dispose();
            
            _eventService.RemoveEventListener<ServerDisconnectedEvent>(ServerHandleServerDisconnected);
            _eventService.RemoveEventListener<ServerReadiedEvent>(ServerHandleServerReadied);
            _eventService.RemoveEventListener<ServerStoppedEvent>(ServerHandleServerStopped);
            _eventService.RemoveEventListener<ClientConnectedEvent>(ClientHandleClientConnected);
            _eventService.RemoveEventListener<ClientStartedEvent>(ClientHandleClientStarted);
            _eventService.RemoveEventListener<ClientStoppedEvent>(ClientHandleClientStopped);
        }

        private void Update()//TODO: JUST UPDATE WHEN THE GAME STATE IS STARTED
        {
            float deltaTime = Time.deltaTime;

            _playerManager.Tick(deltaTime);
            _itemManager.Tick(deltaTime);
        }
        
        [Server]
        private void ServerHandleServerDisconnected(ServiceEvent serviceEvent)
        { }

        [Server]
        private void ServerHandleServerReadied(ServiceEvent serviceEvent)
        {
            if (serviceEvent is ServerReadiedEvent serverReadiedEvent)
            {
                _playerManager.Begin(serverReadiedEvent.Conn);
                
                _cursorManager.TargetRpcLockCursor(serverReadiedEvent.Conn);

                _itemManager.Begin(serverReadiedEvent.Conn);
            }
        }

        [Server]
        private void ServerHandleServerStopped(ServiceEvent serviceEvent)
        {
            _itemManager.Stop();
        }
        
        [Client]
        private void ClientHandleClientConnected(ServiceEvent serviceEvent)
        { }
       
        [Client]
        private void ClientHandleClientStarted(ServiceEvent serviceEvent)
        {
            _voiceListener.Begin();
        }
    
        [Client]
        private void ClientHandleClientStopped(ServiceEvent serviceEvent)
        {
            _playerManager.Stop();
            _itemManager.Stop();
        }
    }
}