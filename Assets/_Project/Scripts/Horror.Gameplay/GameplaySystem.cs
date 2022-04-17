using Horror.Gameplay.VoiceRecognizer;
using Horror.Gameplay.Playing;
using Horror.Gameplay.Events;
using Horror.Gameplay.Items;
using Horror.ServiceLocator;
using Multiplayer.Events;
using Horror.Events;
using Horror.Pooling;
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
            
            _eventService.AddEventListener<ServerReadiedEvent>(ServerHandleServerReadied);
            _eventService.AddEventListener<ServerStoppedEvent>(ServerHandleServerStopped);
            _eventService.AddEventListener<ClientStartedEvent>(ClientHandleClientStarted);
            _eventService.AddEventListener<ClientStoppedEvent>(ClientHandleClientStopped);
            _eventService.AddEventListener<ServerDisconnectedEvent>(ServerHandleServerDisconnected);
        }

        private void OnDestroy()
        {
            _playerManager.Dispose();
            _cursorManager.Dispose();
            _itemManager.Dispose();
            
            _eventService.RemoveEventListener<ServerReadiedEvent>(ServerHandleServerReadied);
            _eventService.RemoveEventListener<ServerStoppedEvent>(ServerHandleServerStopped);
            _eventService.RemoveEventListener<ClientStartedEvent>(ClientHandleClientStarted);
            _eventService.RemoveEventListener<ClientStoppedEvent>(ClientHandleClientStopped);
            _eventService.RemoveEventListener<ServerDisconnectedEvent>(ServerHandleServerDisconnected);
            
            IVoiceService voiceService = GameServices.GetService<IVoiceService>();//
            
            voiceService.Stop();
        }

        private void Update()//TODO: JUST UPDATE WHEN THE GAME STATE IS STARTED
        {
            float deltaTime = Time.deltaTime;

            _playerManager.Tick(deltaTime);
            _itemManager.Tick(deltaTime);
        }

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
        private void ClientHandleClientStarted(ServiceEvent serviceEvent)
        {
            _voiceListener.Begin();
        }
        
        [Server]
        private void ServerHandleServerDisconnected(ServiceEvent serviceEvent)
        {
            if (serviceEvent is ServerDisconnectedEvent serverDisconnectedEvent)
            {
                _playerManager.RemoveDisconnectedPlayerFromList(serverDisconnectedEvent.Conn);
            }
        }
        
        [Client]
        private void ClientHandleClientStopped(ServiceEvent serviceEvent)
        {
            _playerManager.Stop();
            _itemManager.Stop();
        }
    }
}