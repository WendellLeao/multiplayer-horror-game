using Horror.Inputs;
using UnityEngine;

namespace Horror.Gameplay.Playing
{
    [RequireComponent(typeof(MovementStatusEffect))]
    public sealed class PlayerSprint : MonoBehaviour
    {
        [Header("Player Components")]
        [SerializeField] private PlayerCrouch _playerCrouch;
        
        [Header("Sprint")] 
        [SerializeField] private MovementStatusEffect _movementStatusEffect;
        [SerializeField] private float _velocityMultiplier;
        
        private PlayerAnimationsController _animationsController;
        private IInputService _inputService;

        public void Initialize(IInputService inputService, PlayerAnimationsController animationsController)
        {
            _animationsController = animationsController;

            _inputService = inputService;
            
            _inputService.OnReadPlayerInputs += HandlePlayerInputs;
        }

        public void Dispose()
        {
            _inputService.OnReadPlayerInputs -= HandlePlayerInputs;
        }
		
        public void Tick(float deltaTime)
        {}

        private void HandlePlayerInputs(PlayerInputsData playerInputsData)
        {
            if (playerInputsData.IsSprinting)
            {
                _movementStatusEffect.SetVelocityMultiplier(_velocityMultiplier);
                
                _animationsController.PlayRunningAnimation();

                return;
            }

            _movementStatusEffect.ResetVelocityMultiplier();
            
            _animationsController.StopRunningAnimation();
        }
    }
}
