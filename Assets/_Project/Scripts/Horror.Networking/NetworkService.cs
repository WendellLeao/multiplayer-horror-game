using Horror.Gameplay.Events;
using Horror.ServiceLocator;
using Horror.Events;
using Mirror;

namespace Horror.Networking
{
    public sealed class NetworkService : NetworkManager, INetworkService
    {
        public override void Awake()
        {
            base.Awake();

            GameServices.RegisterService<INetworkService>(this);
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
            
            IEventService eventService = GameServices.GetService<IEventService>();
            
            eventService.DispatchEvent(serverReadiedEvent);
        }

        [Server]
        public override void OnStartServer()
        {
            base.OnStartServer();
            
            ServerStartedEvent serverStartedEvent = new ServerStartedEvent();
            
            IEventService eventService = GameServices.GetService<IEventService>();
            
            eventService.DispatchEvent(serverStartedEvent);
        }

        [Server]
        public override void OnStopServer()
        {
            base.OnStopServer();
            
            ServerStoppedEvent serverStoppedEvent = new ServerStoppedEvent();
            
            IEventService eventService = GameServices.GetService<IEventService>();
            
            eventService.DispatchEvent(serverStoppedEvent);
        }

        [Server]
        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            base.OnServerDisconnect(conn);
            
            ServerDisconnectedEvent serverDisconnectedEvent = new ServerDisconnectedEvent(conn);
            
            IEventService eventService = GameServices.GetService<IEventService>();
            
            eventService.DispatchEvent(serverDisconnectedEvent);
        }

        [Client]
        public override void OnStartClient()
        {
            base.OnStartClient();
            
            ClientStartedEvent clientStartedEvent = new ClientStartedEvent();
            
            IEventService eventService = GameServices.GetService<IEventService>();
            
            eventService.DispatchEvent(clientStartedEvent);
        }
        
        [Client]
        public override void OnStopClient()
        {
            base.OnStopClient();
            
            ClientStoppedEvent clientStoppedEvent = new ClientStoppedEvent();
            
            IEventService eventService = GameServices.GetService<IEventService>();
            
            eventService.DispatchEvent(clientStoppedEvent);
        }
    }
}