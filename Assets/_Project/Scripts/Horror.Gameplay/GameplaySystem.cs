using Horror.Gameplay.VoiceRecognizer;
using Horror.Networking.Events;
using Horror.Gameplay.Playing;
using Horror.Gameplay.Items;
using Horror.ServiceLocator;
using Horror.Gameplay.UI;
using System.Collections;
using Horror.UI.Screens;
using Horror.Events;
using UnityEngine;
using Horror.UI;
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
        private IUIService _uiService;

        private void Awake()
        {
            _playerManager.Initialize();
            _cursorManager.Initialize();
            _itemManager.Initialize();

            _eventService = GameServices.GetService<IEventService>();
            
            _eventService.AddEventListener<ServerDisconnectedEvent>(ServerHandleServerDisconnected);
            _eventService.AddEventListener<ServerReadiedEvent>(ServerHandleServerReadied);
            _eventService.AddEventListener<ServerStoppedEvent>(ServerHandleServerStopped);
            _eventService.AddEventListener<ClientStartedEvent>(ClientHandleClientStarted);
            _eventService.AddEventListener<ClientStoppedEvent>(ClientHandleClientStopped);

            _uiService = GameServices.GetService<IUIService>();
            
            UIScreen loadingScreen = _uiService.CurrentOpenedScreen;
            
            loadingScreen.OnClosed += HandleLoadingScreenClosed;
            
            _uiService.OpenScreen<PlayerHUD>(OpenScreenMode.Single, 2f);//TODO: FIX HARD CODE
        }

        private void OnDestroy()
        {
            _playerManager.Dispose();
            _cursorManager.Dispose();
            _itemManager.Dispose();
            
            _eventService.RemoveEventListener<ServerDisconnectedEvent>(ServerHandleServerDisconnected);
            _eventService.RemoveEventListener<ServerReadiedEvent>(ServerHandleServerReadied);
            _eventService.RemoveEventListener<ServerStoppedEvent>(ServerHandleServerStopped);
            _eventService.RemoveEventListener<ClientStartedEvent>(ClientHandleClientStarted);
            _eventService.RemoveEventListener<ClientStoppedEvent>(ClientHandleClientStopped);
        }

        private void Update()
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

        private void HandleLoadingScreenClosed(UIScreen uiScreen)
        {
            _uiService.OpenScreen<PlayerHUD>();

            uiScreen.OnClosed -= HandleLoadingScreenClosed;
        }
    }
}