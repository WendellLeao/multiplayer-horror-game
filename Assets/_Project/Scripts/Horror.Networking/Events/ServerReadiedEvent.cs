using Horror.Events;
using Mirror;

namespace Horror.Networking.Events
{
    public sealed class ServerReadiedEvent : ServiceEvent
    {
        public ServerReadiedEvent(NetworkConnectionToClient conn)
        {
            Conn = conn;
        }
        
        public NetworkConnectionToClient Conn { get; }
    }
}