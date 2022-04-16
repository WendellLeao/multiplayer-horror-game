using System.Collections.Generic;
using UnityEngine;

namespace Horror.Gameplay.ObjectPooling
{
	public static class ObjectPooler
	{
		private static Dictionary<PoolType, Queue<GameObject>> _poolDictionary;

		private static Pool[] _pools;

		public static void Initialize(Pool[] pools)
		{
			_pools = pools;

			_poolDictionary = new Dictionary<PoolType, Queue<GameObject>>();

			foreach (Pool pool in pools)
			{
				Queue<GameObject> objectPool = new Queue<GameObject>();

				for (int i = 0; i < pool.StartAmount; i++)
				{
					GameObject newGameObject = CreateNewObject(pool.ObjectToPool);

					objectPool.Enqueue(newGameObject);
				}

				_poolDictionary.Add(pool.PoolType, objectPool);
			}
		}

		public static GameObject GetObjectFromPool(PoolType poolType)
		{
			if (_poolDictionary.TryGetValue(poolType, out Queue<GameObject> objectList))
			{
				if (objectList.Count == 0)
				{
					return CreateBackupObject(poolType);
				}

				GameObject objectFromPool = objectList.Dequeue();

				objectFromPool.SetActive(true);

				return objectFromPool;
			}

			return null;
		}

		public static void ReturnObjectToPool(PoolType objectType, GameObject objectToReturn)
		{
			if (_poolDictionary.TryGetValue(objectType, out Queue<GameObject> objectList))
			{
				objectList.Enqueue(objectToReturn);
			}

			objectToReturn.SetActive(false);
		}

		private static GameObject CreateNewObject(GameObject gameObject)
		{
			GameObject newGameObject = Object.Instantiate(gameObject);

			newGameObject.SetActive(false);

			return newGameObject;
		}

		private static GameObject CreateBackupObject(PoolType poolType)
		{
			GameObject newBackupObject = null;

			foreach (Pool pool in _pools)
			{
				if (pool.PoolType == poolType)
				{
					newBackupObject = Object.Instantiate(pool.ObjectToPool);

					return newBackupObject;
				}
			}

			return null;
		}
	}
}
