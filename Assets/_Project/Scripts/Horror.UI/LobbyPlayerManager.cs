using Horror.Networking.Events;
using Horror.ServiceLocator;
using Horror.Events;
using UnityEngine;
using System;
using Mirror;

namespace Horror.UI.Lobby
{
    public sealed class LobbyPlayerManager : NetworkBehaviour
    {
        public event Action<LobbyPlayer> OnLobbyPlayerCreated;
        
        [Header("Spawn")]
        [SerializeField] private Transform[] _spawnPoint;
        [SerializeField] private GameObject _lobbyPlayerPrefab;
        
        private const string PlayerNameKey = "PlayerName";

        [SyncVar]
        private int _spawnPointIterator;
        
        private LobbyPlayer _lobbyPlayer;

        private void Awake()
        {
            IEventService eventService = GameServices.GetService<IEventService>();
            
            eventService.AddEventListener<ServerDisconnectedEvent>(ServerHandleServerDisconnected);
            eventService.AddEventListener<ServerReadiedEvent>(ServerHandleServerReadied);
            eventService.AddEventListener<ServerChangeEvent>(ServerHandleChangeScene);
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
            eventService.RemoveEventListener<ServerReadiedEvent>(ServerHandleServerReadied);
            eventService.RemoveEventListener<ServerChangeEvent>(ServerHandleChangeScene);
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
            _spawnPointIterator--;
        }

        [Server]
        private void ServerHandleServerReadied(ServiceEvent serviceEvent)
        {
            if (serviceEvent is ServerReadiedEvent serverReadiedEvent)
            {
                ServerCreateLobbyPlayer(serverReadiedEvent.Conn);
            }
        }

        [Server]
        private void ServerCreateLobbyPlayer(NetworkConnectionToClient conn)
        {
            GameObject lobbyPlayerObject = Instantiate(_lobbyPlayerPrefab);
            
            Transform spawnPoint = _spawnPoint[_spawnPointIterator]; 
            
            lobbyPlayerObject.transform.position = spawnPoint.position;
            lobbyPlayerObject.transform.rotation = spawnPoint.rotation;

            NetworkServer.AddPlayerForConnection(conn, lobbyPlayerObject);

            TargetRpcInitializeLobbyPlayer(conn, lobbyPlayerObject);

            _spawnPointIterator++;

            LobbyPlayer lobbyPlayer = lobbyPlayerObject.GetComponent<LobbyPlayer>();
            
            OnLobbyPlayerCreated?.Invoke(lobbyPlayer);
        }

        [TargetRpc]
        private void TargetRpcInitializeLobbyPlayer(NetworkConnection conn, GameObject lobbyPlayerObject)
        {
            _lobbyPlayer = lobbyPlayerObject.GetComponent<LobbyPlayer>();

            string playerName = PlayerPrefs.GetString(PlayerNameKey);
            
            _lobbyPlayer.Initialize(playerName, conn.identity.isServer);
        }

        private void ServerHandleChangeScene(ServiceEvent serviceEvent)
        {
            _lobbyPlayer.Dispose();
        }
    }
}