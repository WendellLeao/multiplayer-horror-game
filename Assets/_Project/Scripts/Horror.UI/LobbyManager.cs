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

        [Header("UI")] 
        [SerializeField] private LobbyScreen _lobbyScreenData;

        private const string ReadyButtonLabel = "Ready";
        private const string UnreadyButtonLabel = "Unready";
        
        private INetworkService _networkService;
        private LobbyScreen _lobbyScreen;
        private int _readyPlayersCount;
        private bool _isReady;
        private int _iterator;

        private void Awake()
        {
            _networkService = GameServices.GetService<INetworkService>();
            
            IEventService eventService = GameServices.GetService<IEventService>();
            
            eventService.AddEventListener<ServerDisconnectedEvent>(ServerHandleServerDisconnected);
            eventService.AddEventListener<ClientConnectedEvent>(ClientHandleClientConnected);
            eventService.AddEventListener<ServerReadiedEvent>(ServerHandleServerReadied);

            IUIService uiService = GameServices.GetService<IUIService>();
            
            uiService.Clear();

            _lobbyScreen = (LobbyScreen) uiService.OpenScreen(_lobbyScreenData);
            
            _lobbyScreen.OnPlayButtonClicked += HandlePlayButtonClicked;
            _lobbyScreen.OnReadyButtonClicked += HandleReadyButtonClicked;
        }

        private void OnDestroy()
        {
            IEventService eventService = GameServices.GetService<IEventService>();
            
            eventService.RemoveEventListener<ServerDisconnectedEvent>(ServerHandleServerDisconnected);
            eventService.RemoveEventListener<ClientConnectedEvent>(ClientHandleClientConnected);
            eventService.RemoveEventListener<ServerReadiedEvent>(ServerHandleServerReadied);
            
            _lobbyScreen.OnPlayButtonClicked -= HandlePlayButtonClicked;
            _lobbyScreen.OnReadyButtonClicked -= HandleReadyButtonClicked;
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

            _iterator++;
        }
        
        [Client]
        private void ClientHandleClientConnected(ServiceEvent serviceEvent)
        {
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
                CmdIncreaseReadyPlayersCount();

                _isReady = true;
                
                _lobbyScreen.SetReadyButtonLabelText(UnreadyButtonLabel);

                return;
            }

            CmdDecreaseReadyPlayersCount();
            
            _isReady = false;
            
            _lobbyScreen.SetReadyButtonLabelText(ReadyButtonLabel);
        }

        [Command(requiresAuthority = false)]
        private void CmdIncreaseReadyPlayersCount()
        {
            _readyPlayersCount++;

            CheckAndSetPlayButtonInteractable();
        }
        
        [Command(requiresAuthority = false)]
        private void CmdDecreaseReadyPlayersCount()
        {
            _readyPlayersCount--;
            
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
