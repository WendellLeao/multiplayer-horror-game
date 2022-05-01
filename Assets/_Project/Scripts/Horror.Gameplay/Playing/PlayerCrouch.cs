using Horror.Gameplay.Cameras;
using Horror.Inputs;
using UnityEngine;

namespace Horror.Gameplay.Playing
{
    [RequireComponent(typeof(MovementStatusEffect))]
    public sealed class PlayerCrouch : MonoBehaviour
    {
        [Header("Player Components")]
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private CapsuleCollider _capsuleCollider;
        [SerializeField] private Transform _cameraTarget;
        
        [Header("Crouch")]
        [SerializeField] private float _crouchHeight;
        [SerializeField] private Vector3 _crouchCenter;
        
        private PlayerAnimationsController _animationsController;
        private ICamera _firstPersonCamera;
        private IInputService _inputService;
        private Transform _originalCameraTarget;
        private Vector3 _originalCharacterCenter;
        private float _originalCharacterHeight;
        private bool _isCrouching;

        public void Initialize(IInputService inputService, PlayerAnimationsController animationsController,
            ICamera firstPersonCamera)
        {
            _firstPersonCamera = firstPersonCamera;
            
            _originalCameraTarget = _firstPersonCamera.CameraTarget;
                
            _originalCharacterHeight = _characterController.height;
            _originalCharacterCenter = _characterController.center;
            
            SetCharacterHeight(_originalCharacterHeight);
            SetCapsuleColliderHeight(_originalCharacterHeight);
            
            _animationsController = animationsController;

            _inputService = inputService;
            
            _inputService.OnReadPlayerInputs += HandleReadPlayerInputs;
        }

        public void Dispose()
        {
            _inputService.OnReadPlayerInputs -= HandleReadPlayerInputs;
        }

        public void Tick(float deltaTime)
        { }
        
        private void HandleReadPlayerInputs(PlayerInputsData playerInputsData)
        {
            if (playerInputsData.IsCrouching)
            {
                _isCrouching = !_isCrouching;

                HandleCrouch(_isCrouching);
            }
        }

        private void HandleCrouch(bool isCrouching)
        {
            if (isCrouching)
            {
                _firstPersonCamera.SetTarget(_cameraTarget);
                
                SetCapsuleColliderHeight(_crouchHeight);
                SetCapsuleColliderCenter(_crouchCenter);
                
                SetCharacterHeight(_crouchHeight);
                SetCharacterCenter(_crouchCenter);
                
                _animationsController.PlayCrouchingAnimation();
                
                return;
            }

            _firstPersonCamera.SetTarget(_originalCameraTarget);
            
            SetCapsuleColliderHeight(_originalCharacterHeight);
            SetCapsuleColliderCenter(_originalCharacterCenter);
            
            SetCharacterHeight(_originalCharacterHeight);
            SetCharacterCenter(_originalCharacterCenter);
            
            _animationsController.StopCrouchingAnimation();
        }

        private void SetCharacterHeight(float height)
        {
            _characterController.height = height;
        }
        
        private void SetCapsuleColliderHeight(float height)
        {
            _capsuleCollider.height = height;
        }

        private void SetCapsuleColliderCenter(Vector3 newCenter)
        {
            _capsuleCollider.center = newCenter;
        }
        
        private void SetCharacterCenter(Vector3 newCenter)
        {
            _characterController.center = newCenter;
        }
    }
}
