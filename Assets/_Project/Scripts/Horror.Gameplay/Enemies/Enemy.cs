using Horror.Gameplay.Enemies.EnemyAssemblies;
using Horror.Gameplay.Evidences;
using UnityEngine;

namespace Horror.Gameplay.Enemies
{
    public abstract class Enemy : NetworkEntity
    {
        [SerializeField] private EnemyView _enemyView;
        
        private EnemyAssemblyData _enemyAssemblyData;
        private EnemyData _enemyData;

        public void Begin(EnemyData enemyData)
        {
            _enemyData = enemyData;

            Debug.Log(_enemyData.name);
            
            foreach (EvidenceData enemyDataEvidence in _enemyData.Evidences)
            {
                Debug.Log(enemyDataEvidence.ID);
            }
            
            SetEnemyAssemblyData(_enemyData);

            _enemyView.Setup(_enemyAssemblyData);
        }

        private void SetEnemyAssemblyData(EnemyData enemyData)
        {
            EnemyAssemblyData[] enemyAssembly = enemyData.EnemyAssemblyDatas;
            
            int randomIndex = Random.Range(0, enemyAssembly.Length);

            _enemyAssemblyData = enemyAssembly[randomIndex];
        }
    }
}
