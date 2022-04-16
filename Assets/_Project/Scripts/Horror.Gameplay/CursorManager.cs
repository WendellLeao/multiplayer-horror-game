using Horror.ServiceLocator;
using Horror.Inputs;
using UnityEngine;
using Mirror;

namespace Horror.Gameplay
{
    public sealed class CursorManager : NetworkBehaviour
    {
        private IInputService _inputService;
        private bool _cursorIsLocked;

        public void Initialize()
        {
            _inputService = GameServices.GetService<IInputService>();
            
            _inputService.OnReadPlayerInputs += HandlePlayerInputs;
        }

        public void Dispose()
        {
            _inputService.OnReadPlayerInputs -= HandlePlayerInputs;
        }

        [TargetRpc]
        public void TargetRpcLockCursor(NetworkConnection conn)
        {
            LockCursor();
        }
        
        [TargetRpc]
        public void TargetRpcUnlockCursor(NetworkConnection conn)
        {
            UnlockCursor();
        }
        
        private void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            _cursorIsLocked = true;
        }

        private void UnlockCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            _cursorIsLocked = false;
        }

        private void HandlePlayerInputs(PlayerInputsData playerInputsData)
        {
            if (playerInputsData.PressESC)
            {
                if (_cursorIsLocked)
                {
                    UnlockCursor();

                    return;
                }
                
                LockCursor();
            }
        }
    }
}