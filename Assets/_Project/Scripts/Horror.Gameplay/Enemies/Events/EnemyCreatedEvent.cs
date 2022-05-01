using Horror.Events;

namespace Horror.Gameplay.Enemies.Events
{
    public sealed class EnemyCreatedEvent : ServiceEvent
    {
        public EnemyCreatedEvent(Enemy enemy)
        {
            Enemy = enemy;
        }
        
        public Enemy Enemy { get; }
    }
}