using System;

namespace Horror.Inputs
{
    public interface IInputService
    {
        event Action<PlayerInputsData> OnReadPlayerInputs;
        void DispatchPlayerInputs(PlayerInputsData playerInputsData);
    }
}