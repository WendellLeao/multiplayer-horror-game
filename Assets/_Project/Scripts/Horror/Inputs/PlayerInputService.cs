using Horror.ServiceLocator;
using UnityEngine;
using System;

namespace Horror.Inputs
{
    public sealed class PlayerInputService : IInputService
    {
        public event Action<PlayerInputsData> OnReadPlayerInputs;
        
        public void DispatchPlayerInputs(PlayerInputsData playerInputsData)
        {
            OnReadPlayerInputs?.Invoke(playerInputsData);
        }
    }
}
