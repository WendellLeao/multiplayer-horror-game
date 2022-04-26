using System.Collections.Generic;
using Horror.Networking.Events;
using Horror.UI.Screens.Lobby;
using Horror.ServiceLocator;
using Horror.Networking;
using Horror.UI.Screens;
using Horror.Events;
using UnityEngine;
using Mirror;

namespace Horror.UI.Lobby
{
    public sealed class LobbyManager : NetworkBehaviour
    {
        [Scene]
        [SerializeField] private string _gameSceneName;

        [Header("Components")]
        [SerializeField] private LobbyPlayerManager _lobbyPlayerManager;
        
        private const string UnreadyButtonLabel = "Unready";
        private const string ReadyButtonLabel = "Ready";

        [SyncVar]
        private int _lobbyPlayerIterator;
        
        private readonly List<LobbyPlayer> _lobbyPlayers = new List<LobbyPlayer>();
        private INetworkService _networkService;
        private LobbyScreen _lobbyScreen;
        private IUIService _uiService;
        private int _readyPlayersCount;
        private bool _hasInitialized;
        private bool _isReady;

        private void Awake()
        {
            _uiService = GameServices.GetService<IUIService>();

            _networkService = GameServices.GetService<INetworkService>();
            
            IEventService eventService = GameServices.GetService<IEventService>();
            
            eventService.AddEventListener<ServerDisconnectedEvent>(ServerHandleServerDisconnected);
            eventService.AddEventListener<ClientConnectedEvent>(ClientHandleClientConnected);
            eventService.AddEventListener<ServerReadiedEvent>(ServerHandleServerReadied);
            
            _uiService = GameServices.GetService<IUIService>();
            
            _lobbyScreen = (LobbyScreen) _uiService.GetRegisteredScreen<LobbyScreen>();
            
            _lobbyScreen.OnPlayButtonClicked += HandlePlayButtonClicked;
            _lobbyScreen.OnReadyButtonClicked += HandleReadyButtonClicked;
            _lobbyScreen.OnBackButtonClicked += HandleBackButtonClicked;
            
            _uiService.CurrentOpenedScreen.OnClosed += HandleLoadingScreenClosed;
            
            _lobbyPlayerManager.OnLobbyPlayerCreated += HandleLobbyPlayerCreated;
            
            _hasInitialized = true;
        }

        private void OnDestroy()
        {
            IEventService eventService = GameServices.GetService<IEventService>();
            
            eventService.RemoveEventListener<ServerDisconnectedEvent>(ServerHandleServerDisconnected);
            eventService.RemoveEventListener<ClientConnectedEvent>(ClientHandleClientConnected);
            eventService.RemoveEventListener<ServerReadiedEvent>(ServerHandleServerReadied);
            
            _lobbyScreen.OnPlayButtonClicked -= HandlePlayButtonClicked;
            _lobbyScreen.OnReadyButtonClicked -= HandleReadyButtonClicked;
            _lobbyScreen.OnBackButtonClicked -= HandleBackButtonClicked;
        }

        [Server]
        private void ServerHandleServerDisconnected(ServiceEvent serviceEvent)
        {
            _lobbyPlayers.RemoveAt(_lobbyPlayerIterator - 1);
            
            _lobbyPlayerIterator--;
        }

        [Server]
        private void ServerHandleServerReadied(ServiceEvent serviceEvent)
        {
            CheckAndSetPlayButtonInteractable();
        }

        [Client]
        private void ClientHandleClientConnected(ServiceEvent serviceEvent)
        {
            if (!_hasInitialized)
            {
                return;
            }
            
            if (isServer)
            {
                _lobbyScreen.ActiveHostButtonsGroup();

                return;
            }
            
            _lobbyScreen.ActiveClientButtonsGroup();
        }
        
        [Server]
        private void HandlePlayButtonClicked()
        {
            if (!CanStartTheGame())
            {
                return;
            }

            _uiService.OpenScreen<LoadingScreen>();
            
            NetworkServer.Destroy(_lobbyPlayerManager.gameObject);
            NetworkServer.Destroy(gameObject);
            
            _networkService.ServerChangeScene(_gameSceneName);
        }

        [Client]
        private void HandleReadyButtonClicked()
        {
            if (!_isReady)
            {
                _isReady = true;

                CmdUpdateReadiness(_isReady, _lobbyPlayerIterator);

                _lobbyScreen.SetReadyButtonLabelText(UnreadyButtonLabel);

                return;
            }

            _isReady = false;

            CmdUpdateReadiness(_isReady, _lobbyPlayerIterator);

            _lobbyScreen.SetReadyButtonLabelText(ReadyButtonLabel);
        }

        [Command(requiresAuthority = false)]
        private void CmdUpdateReadiness(bool isReady, int iterator)
        {
            if (isReady)
            {
                _readyPlayersCount++;
            }
            else
            {
                _readyPlayersCount--;
            }
            
            LobbyPlayer lobbyPlayer = _lobbyPlayers[iterator - 1];
            
            lobbyPlayer.CmdUpdateReadiness(isReady);
            
            CheckAndSetPlayButtonInteractable();
        }

        [Server]
        private void CheckAndSetPlayButtonInteractable()
        {
            bool canStartTheGame = CanStartTheGame();

            _lobbyScreen.SetPlayButtonInteractable(canStartTheGame);
        }

        [Server]
        private void HandleLobbyPlayerCreated(LobbyPlayer lobbyPlayer)
        {
            _lobbyPlayers.Add(lobbyPlayer);
            
            _lobbyPlayerIterator++;

            CheckAndSetPlayButtonInteractable();
            
            RpcCloseLoadingScreen(lobbyPlayer.connectionToClient);
        }

        [TargetRpc]
        private void RpcCloseLoadingScreen(NetworkConnection conn)
        {
            _uiService.CloseTopScreen();
        }

        private void HandleLoadingScreenClosed(UIScreen uiScreen)
        {
            _uiService.OpenScreen(_lobbyScreen);

            uiScreen.OnClosed -= HandleLoadingScreenClosed;
        }

        private void HandleBackButtonClicked()
        {
            _uiService.OpenScreen<LoadingScreen>();
            
            if (isServer)
            {
                _networkService.StopHost();

                return;
            }
            
            _networkService.StopClient();
        }
        
        private bool CanStartTheGame()
        {
            int clientsConnectedCountExceptHost = _networkService.ConnectedPlayersCount - 1;
            
            if (_readyPlayersCount >= clientsConnectedCountExceptHost)
            {
                return true;
            }

            return false;
        }
    }
}
