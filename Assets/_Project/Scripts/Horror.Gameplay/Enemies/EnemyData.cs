using Horror.Gameplay.Enemies.EnemyAssemblies;
using Horror.Gameplay.Evidences;
using UnityEngine;

namespace Horror.Gameplay.Enemies
{
    [CreateAssetMenu(menuName = "Enemies/EnemyData", fileName = "NewGameData")]
    public sealed class EnemyData : ScriptableObject
    {
        public GameObject EnemyPrefab;
     
        [Space(5f)]
        public EvidenceType[] Evidences;
        public EnemyAssemblyData[] EnemyAssemblyDatas;
    }
}