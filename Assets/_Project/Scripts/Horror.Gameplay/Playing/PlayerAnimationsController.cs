using UnityEngine;
using System;

namespace Horror.Gameplay.Playing
{
    public sealed class PlayerAnimationsController : MonoBehaviour
    {
        public event Action<PlayerAnimationsData> OnAnimationsDataChanged;
        
        [SerializeField] private Animator _animator;
        [SerializeField] private AnimatorOverrideController _originalAnimator;
        [SerializeField] private AnimatorOverrideController _carrierAnimator;
        
        private static readonly int HorizontalMovementHash = Animator.StringToHash("HorizontalMovement");
        private static readonly int VerticalMovementHash = Animator.StringToHash("VerticalMovement");
        private static readonly int IsCrouchingHash = Animator.StringToHash("IsCrouching");
        private static readonly int IsRunningHash = Animator.StringToHash("IsRunning");
        private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");

        private PlayerAnimationsData _animationsData;
        private float _minimumAnimationFloat = 0.05f;

        public void Initialize()
        {
            SetOriginalAnimatorController();
        }
        
        public void UpdateMovementAnimation(float horizontalMovement, float verticalMovement)
        {
            bool isMoving = IsMoving(horizontalMovement, verticalMovement);
            
            _animator.SetBool(IsMovingHash, isMoving);
            
            _animator.SetFloat(HorizontalMovementHash, horizontalMovement);
            _animator.SetFloat(VerticalMovementHash, verticalMovement);

            _animationsData.HorizontalMovement = horizontalMovement;
            _animationsData.VerticalMovement = verticalMovement;
            _animationsData.IsMoving = isMoving;
            
            OnAnimationsDataChanged?.Invoke(_animationsData);
        }

        public void PlayRunningAnimation()
        {
            _animationsData.IsRunning = true;
            
            _animator.SetBool(IsRunningHash, _animationsData.IsRunning);
            
            OnAnimationsDataChanged?.Invoke(_animationsData);
        }

        public void StopRunningAnimation()
        {
            _animationsData.IsRunning = false;
            
            _animator.SetBool(IsRunningHash, _animationsData.IsRunning);
            
            OnAnimationsDataChanged?.Invoke(_animationsData);
        }
        
        public void PlayCrouchingAnimation()
        {
            _animationsData.IsCrunching = true;
            
            _animator.SetBool(IsCrouchingHash, _animationsData.IsCrunching);
            
            OnAnimationsDataChanged?.Invoke(_animationsData);
        }

        public void StopCrouchingAnimation()
        {
            _animationsData.IsCrunching = false;
            
            _animator.SetBool(IsCrouchingHash, _animationsData.IsCrunching);
            
            OnAnimationsDataChanged?.Invoke(_animationsData);
        }

        private bool IsMoving(float horizontalMovement, float verticalMovement)
        {
            bool isMovingHorizontally = Mathf.Abs(horizontalMovement) > _minimumAnimationFloat;
            bool isMovingVertically = Mathf.Abs(verticalMovement) > _minimumAnimationFloat;

            return isMovingHorizontally || isMovingVertically;
        }

        public void SetCarrierAnimatorController()
        {
            _animator.runtimeAnimatorController = _carrierAnimator;
        }
        
        public void SetOriginalAnimatorController()
        {
            _animator.runtimeAnimatorController = _originalAnimator;
        }
    }
}