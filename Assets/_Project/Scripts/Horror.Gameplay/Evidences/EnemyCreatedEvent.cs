using Horror.Gameplay.Enemies;
using Horror.Events;

namespace Horror.Gameplay.Evidences
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