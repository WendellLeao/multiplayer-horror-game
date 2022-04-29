using Horror.Gameplay.Evidences;
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

            RpcCreateEnemy(randomIndex);
        }

        [ClientRpc]
        private void RpcCreateEnemy(int randomIndex)
        {
            EnemyData randomEnemyData = _enemies[randomIndex];

            GameObject enemyClone = Instantiate(randomEnemyData.EnemyPrefab);

            NetworkServer.Spawn(enemyClone);
            
            Enemy enemy = enemyClone.GetComponent<Enemy>();

            enemy.Begin(randomEnemyData);

            IEventService eventService = GameServices.GetService<IEventService>();
            
            eventService.DispatchEvent(new EnemyCreatedEvent(enemy));
        }
    }
}
