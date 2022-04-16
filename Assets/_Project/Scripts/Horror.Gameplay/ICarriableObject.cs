using UnityEngine;
using Mirror;

namespace Horror.Gameplay
{
    public interface ICarriableObject
    {
        NetworkIdentity NetIdentity { get; }
        bool CanBePickedUp { get; }

        void SetContainer(Transform container);
        void Begin(Carrier carrier);
        void ExecuteAction();
        void Throw();
    }
}
