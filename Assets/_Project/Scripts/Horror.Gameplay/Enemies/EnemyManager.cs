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

            IVoiceService voiceService = GameServices.GetService<IVoiceService>();            

            enemy.Begin(randomEnemyData, voiceService);

            DispatchEnemyCreatedEvent(enemy);
        }

        private static void DispatchEnemyCreatedEvent(Enemy enemy)
        {
            IEventService eventService = GameServices.GetService<IEventService>();

            eventService.DispatchEvent(new EnemyCreatedEvent(enemy));
        }
    }
}
