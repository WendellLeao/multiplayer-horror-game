using Horror.Gameplay.Enemies.EnemyAssemblies;
using UnityEngine;

namespace Horror.Gameplay.Enemies
{
    public sealed class EnemyView : MonoBehaviour
    {
        public void Setup(EnemyAssemblyData enemyAssemblyData)
        {
            Instantiate(enemyAssemblyData.Model);
            
            Debug.Log(enemyAssemblyData.Sex);            
            Debug.Log(enemyAssemblyData.Age);            
            Debug.Log(enemyAssemblyData.Voice);
        }
    }
}