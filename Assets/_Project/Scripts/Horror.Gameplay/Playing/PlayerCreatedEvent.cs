using Horror.Events;

namespace Horror.Gameplay.Playing
{
    public class PlayerCreatedEvent : ServiceEvent
    {
        public PlayerCreatedEvent(Player player)
        {
            Player = player;
        }
        
        public Player Player { get; }
    }
}