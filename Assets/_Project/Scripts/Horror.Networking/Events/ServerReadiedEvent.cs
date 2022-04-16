using Multiplayer.Events;
using Mirror;

namespace Horror.Gameplay.Events
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