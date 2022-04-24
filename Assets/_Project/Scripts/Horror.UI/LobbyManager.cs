using System.Collections.Generic;
using Horror.UI.Screens.Lobby;
using Horror.Gameplay.Events;
using Horror.ServiceLocator;
using Multiplayer.Events;
using Horror.Networking;
using Horror.Events;
using UnityEngine;
using Mirror;

namespace Horror.UI.Lobby
{
    public sealed class LobbyManager : NetworkBehaviour
    {
        [Scene]
        [SerializeField] private string _gameSceneName;
        
        [Header("Spawn")]
        [SerializeField] private Transform[] _spawnPoint;
        [SerializeField] private GameObject _lobbyPlayerPrefab;
        
        private const string PlayerNameKey = "PlayerName";
        private const string UnreadyButtonLabel = "Unready";
        private const string ReadyButtonLabel = "Ready";

        [SyncVar]
        private int _iterator;
        
        private List<LobbyPlayer> _lobbyPlayers = new List<LobbyPlayer>();
        private INetworkService _networkService;
        private LobbyScreen _lobbyScreen;
        private LobbyPlayer _lobbyPlayer;
        private int _readyPlayersCount;
        private bool _hasInitialized;
        private bool _isReady;

        private void Awake()
        {
            _networkService = GameServices.GetService<INetworkService>();
            
            IEventService eventService = GameServices.GetService<IEventService>();
            
            eventService.AddEventListener<ServerDisconnectedEvent>(ServerHandleServerDisconnected);
            eventService.AddEventListener<ClientConnectedEvent>(ClientHandleClientConnected);
            eventService.AddEventListener<ServerReadiedEvent>(ServerHandleServerReadied);
            
            IUIService uiService = GameServices.GetService<IUIService>();
            
            _lobbyScreen = (LobbyScreen) uiService.OpenScreen<LobbyScreen>();
            
            _lobbyScreen.OnPlayButtonClicked += HandlePlayButtonClicked;
            _lobbyScreen.OnReadyButtonClicked += HandleReadyButtonClicked;

            _hasInitialized = true;
        }

        private void OnDestroy()
        {
            if (_lobbyPlayer == null)
            {
                return;
            }
            
            _lobbyPlayer.Dispose();
            
            IEventService eventService = GameServices.GetService<IEventService>();
            
            eventService.RemoveEventListener<ServerDisconnectedEvent>(ServerHandleServerDisconnected);
            eventService.RemoveEventListener<ClientConnectedEvent>(ClientHandleClientConnected);
            eventService.RemoveEventListener<ServerReadiedEvent>(ServerHandleServerReadied);
            
            _lobbyScreen.OnPlayButtonClicked -= HandlePlayButtonClicked;
            _lobbyScreen.OnReadyButtonClicked -= HandleReadyButtonClicked;
        }

        private void Update()
        {
            if (_lobbyPlayer == null)
            {
                return;
            }
            
            _lobbyPlayer.Tick(Time.deltaTime);
        }

        [Server]
        private void ServerHandleServerDisconnected(ServiceEvent serviceEvent)
        {
            _iterator--;
        }

        [Server]
        private void ServerHandleServerReadied(ServiceEvent serviceEvent)
        {
            if (serviceEvent is ServerReadiedEvent serverReadiedEvent)
            {
                ServerCreateLobbyPlayer(serverReadiedEvent.Conn);

                CheckAndSetPlayButtonInteractable();
            }
        }

        [Server]
        private void ServerCreateLobbyPlayer(NetworkConnectionToClient conn)
        {
            GameObject lobbyPlayerObject = Instantiate(_lobbyPlayerPrefab);
            
            Transform spawnPoint = _spawnPoint[_iterator]; 
            
            lobbyPlayerObject.transform.position = spawnPoint.position;
            lobbyPlayerObject.transform.rotation = spawnPoint.rotation;

            NetworkServer.AddPlayerForConnection(conn, lobbyPlayerObject);

            TargetRpcSetupLobbyPlayer(conn, lobbyPlayerObject);

            _lobbyPlayers.Add(lobbyPlayerObject.GetComponent<LobbyPlayer>());

            _iterator++;
        }

        [TargetRpc]
        private void TargetRpcSetupLobbyPlayer(NetworkConnection conn, GameObject lobbyPlayerObject)
        {
            _lobbyPlayer = lobbyPlayerObject.GetComponent<LobbyPlayer>();

            string playerName = PlayerPrefs.GetString(PlayerNameKey);
            
            _lobbyPlayer.Initialize(playerName, conn.identity.isServer);
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
        
        private void HandlePlayButtonClicked()
        {
            if (!CanStartTheGame())
            {
                return;
            }
            
            _networkService.ServerChangeScene(_gameSceneName);
        }

        private void HandleReadyButtonClicked()
        {
            if (!_isReady)
            {
                _isReady = true;

                CmdUpdateReadiness(_isReady, _iterator);

                _lobbyScreen.SetReadyButtonLabelText(UnreadyButtonLabel);

                return;
            }

            _isReady = false;

            CmdUpdateReadiness(_isReady, _iterator);

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

        private void CheckAndSetPlayButtonInteractable()
        {
            bool canStartTheGame = CanStartTheGame();

            _lobbyScreen.SetPlayButtonInteractable(canStartTheGame);
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
