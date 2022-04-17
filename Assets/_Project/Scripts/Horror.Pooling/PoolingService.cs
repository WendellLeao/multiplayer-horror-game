using System.Collections.Generic;
using Horror.ServiceLocator;
using UnityEngine;

namespace Horror.Pooling
{
	public sealed class PoolingService: MonoBehaviour, IPoolingService
	{
		private const string PoolDatasPath = "PoolingService/PoolDatas";
		
		private Dictionary<PoolType, Queue<GameObject>> _poolDictionary;
		private PoolData[] _poolDatas;

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
			GameObject newGameObject = Instantiate(gameObject);

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
					newBackupObject = Instantiate(pool.ObjectToPool);

					return newBackupObject;
				}
			}

			return null;
		}

		private void Awake()
		{
			if (ServiceIsRegistered())
			{
				return;
			}
			
			GameServices.RegisterService<IPoolingService>(this);
			
			_poolDatas = Resources.LoadAll<PoolData>(PoolDatasPath);

			_poolDictionary = new Dictionary<PoolType, Queue<GameObject>>();
			
			PopulateDictionary();
			
			DontDestroyOnLoad(gameObject);
		}

		private void OnDestroy()
		{
			GameServices.DeregisterService<IPoolingService>();
		}
		
		private void PopulateDictionary()
		{
			foreach (PoolData pool in _poolDatas)
			{
				Queue<GameObject> objectPool = new Queue<GameObject>();

				for (int i = 0; i < pool.StartAmount; i++)
				{
					GameObject newGameObject = CreateNewObject(pool.ObjectToPool);

					objectPool.Enqueue(newGameObject);

					newGameObject.transform.SetParent(transform);
				}

				_poolDictionary.Add(pool.PoolType, objectPool);
			}
		}

		private bool ServiceIsRegistered()
		{
			IPoolingService poolingService = GameServices.GetService<IPoolingService>();
			
			if (poolingService != null)
			{
				return true;
			}

			return false;
		}
	}
}
