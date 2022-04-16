using Multiplayer.Events;
using Mirror;

namespace Horror.Gameplay.Events
{
    public sealed class ServerDisconnectedEvent : ServiceEvent
    {
        public ServerDisconnectedEvent(NetworkConnectionToClient conn)
        {
            Conn = conn;
        }
        
        public NetworkConnectionToClient Conn { get; }
    }
}