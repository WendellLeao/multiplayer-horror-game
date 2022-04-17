using Horror.Gameplay.Events;
using Horror.ServiceLocator;
using Horror.Events;
using Mirror;

namespace Horror.Networking
{
    public sealed class NetworkService : NetworkManager, INetworkService
    {
        private IEventService _eventService;

        public override void Awake()
        {
            base.Awake();

            GameServices.RegisterService<INetworkService>(this);
            
            _eventService = GameServices.GetService<IEventService>();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            
            GameServices.DeregisterService<INetworkService>();
        }

        [Server]
        public override void OnServerReady(NetworkConnectionToClient conn)
        {
            base.OnServerReady(conn);
            
            ServerReadiedEvent serverReadiedEvent = new ServerReadiedEvent(conn);
            
            _eventService.DispatchEvent(serverReadiedEvent);
        }

        [Server]
        public override void OnStartServer()
        {
            base.OnStartServer();
            
            ServerStartedEvent serverStartedEvent = new ServerStartedEvent();
            
            _eventService.DispatchEvent(serverStartedEvent);
        }

        [Server]
        public override void OnStopServer()
        {
            base.OnStopServer();
            
            ServerStoppedEvent serverStoppedEvent = new ServerStoppedEvent();
            
            _eventService.DispatchEvent(serverStoppedEvent);
        }

        [Server]
        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            base.OnServerDisconnect(conn);
            
            ServerDisconnectedEvent serverDisconnectedEvent = new ServerDisconnectedEvent(conn);
            
            _eventService.DispatchEvent(serverDisconnectedEvent);
        }

        [Client]
        public override void OnStartClient()
        {
            base.OnStartClient();
            
            ClientStartedEvent clientStartedEvent = new ClientStartedEvent();
            
            _eventService.DispatchEvent(clientStartedEvent);
        }
        
        [Client]
        public override void OnStopClient()
        {
            base.OnStopClient();
            
            ClientStoppedEvent clientStoppedEvent = new ClientStoppedEvent();
            
            _eventService.DispatchEvent(clientStoppedEvent);
        }
    }
}