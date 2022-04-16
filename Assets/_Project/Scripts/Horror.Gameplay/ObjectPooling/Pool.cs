using UnityEngine;

namespace Horror.Gameplay.ObjectPooling
{
	[System.Serializable]
	public sealed class Pool
	{
		public PoolType PoolType;
	
		public GameObject ObjectToPool;
	
		public int StartAmount;
	}
}
