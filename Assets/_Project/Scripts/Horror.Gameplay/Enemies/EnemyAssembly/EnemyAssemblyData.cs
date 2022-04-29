using UnityEngine;

namespace Horror.Gameplay.Enemies.EnemyAssemblies
{
    [CreateAssetMenu(menuName = "Enemies/EnemySetData", fileName = "NewEnemySetData")]
    public sealed class EnemyAssemblyData : ScriptableObject
    {
        public EnemySex Sex;
        public EnemyAge Age;
        public EnemyVoice Voice;
        public GameObject Model;
        //public Mesh Mesh;//TODO: LEARN HOW TO SET MESH AT RUNTIME
    }
}