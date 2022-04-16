using System.Collections.Generic;
using UnityEngine;

namespace Horror.Pooling
{
	public sealed class PoolingService: IPoolingService
	{
		private Dictionary<PoolType, Queue<GameObject>> _poolDictionary;
		private PoolData[] _poolDatas;

		public PoolingService()
		{
			_poolDatas = Resources.LoadAll<PoolData>("PoolingService/PoolData");

			_poolDictionary = new Dictionary<PoolType, Queue<GameObject>>();
			
			foreach (PoolData pool in _poolDatas)
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

		public GameObject GetObjectFromPool(PoolType poolType)
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

		public void ReturnObjectToPool(PoolType objectType, GameObject objectToReturn)
		{
			if (_poolDictionary.TryGetValue(objectType, out Queue<GameObject> objectList))
			{
				objectList.Enqueue(objectToReturn);
			}

			objectToReturn.SetActive(false);
		}

		private GameObject CreateNewObject(GameObject gameObject)
		{
			GameObject newGameObject = Object.Instantiate(gameObject);

			newGameObject.SetActive(false);

			return newGameObject;
		}

		private GameObject CreateBackupObject(PoolType poolType)
		{
			GameObject newBackupObject = null;

			foreach (PoolData pool in _poolDatas)
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
