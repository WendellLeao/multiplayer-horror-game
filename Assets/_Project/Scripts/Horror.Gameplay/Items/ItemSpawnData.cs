using UnityEngine;
using System;

namespace Horror.Gameplay.Items
{
    [Serializable]
    public class ItemSpawnData
    {
        public ItemData ItemData;
        public Transform[] SpawnPoints;

        private int _iterator;

        public Transform CurrentSpawnPoint => SpawnPoints[_iterator];
        
        public void IncreaseIterator()
        {
            _iterator++;
            
            int lastSpawnPointIndex = SpawnPoints.Length - 1;
            
            _iterator = Mathf.Clamp(_iterator, 0, lastSpawnPointIndex);
        }

        public void Reset()
        {
            _iterator = 0;
        }
    }
}