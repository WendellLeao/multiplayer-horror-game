using Horror.Events;
using Mirror;

namespace Horror.Networking.Events
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