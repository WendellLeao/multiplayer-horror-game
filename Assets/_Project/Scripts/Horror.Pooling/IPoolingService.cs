using UnityEngine;

namespace Horror.Pooling
{
    public interface IPoolingService
    {
        void Begin();
        GameObject GetObjectFromPool(PoolType poolType);
        void ReturnObjectToPool(PoolType objectType, GameObject objectToReturn);
    }
}
