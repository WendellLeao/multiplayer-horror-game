using UnityEngine;

namespace Horror.Pooling
{
    public interface IPoolingService
    {
        GameObject GetObjectFromPool(PoolType poolType);
        void ReturnObjectToPool(PoolType objectType, GameObject objectToReturn);
    }
}