using Horror.Events;

namespace Horror.Gameplay.Enemies.Events
{
    public sealed class EnemyResponseEvent : ServiceEvent
    {
        public EnemyResponseEvent(float manifestationDuration)
        {
            ManifestationDuration = manifestationDuration;
        }
        
        public float ManifestationDuration { get; }
    }
}