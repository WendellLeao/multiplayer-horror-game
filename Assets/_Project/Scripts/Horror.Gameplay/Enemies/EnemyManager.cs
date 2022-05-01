using Horror.Gameplay.VoiceRecognizer;
using Horror.Gameplay.Enemies.Events;
using Horror.ServiceLocator;
using Horror.Events;
using UnityEngine;
using Mirror;

namespace Horror.Gameplay.Enemies
{
    public sealed class EnemyManager : NetworkBehaviour
    {
        [SerializeField] private EnemyData[] _enemies;
        
        private IEventService _eventService;

        public void Initialize(IEventService eventService)
        {
            _eventService = eventService;
        }
        
        [Server]
        public void Begin()
        {
            int randomIndex = Random.Range(0, _enemies.Length);

            EnemyData randomEnemyData = _enemies[randomIndex];
            
            GameObject enemyClone = Instantiate(randomEnemyData.EnemyPrefab);
            
            NetworkServer.Spawn(enemyClone);
            
            RpcCreateEnemy(enemyClone, randomIndex);
        }

        [ClientRpc]
        private void RpcCreateEnemy(GameObject enemyClone, int randomIndex)
        {
            EnemyData randomEnemyData = _enemies[randomIndex];
            
            Enemy enemy = enemyClone.GetComponent<Enemy>();

            enemy.Begin(randomEnemyData, _eventService);

            DispatchEnemyCreatedEvent(enemy);
        }

        private static void DispatchEnemyCreatedEvent(Enemy enemy)
        {
            IEventService eventService = GameServices.GetService<IEventService>();

            eventService.DispatchEvent(new EnemyCreatedEvent(enemy));
        }
    }
}
