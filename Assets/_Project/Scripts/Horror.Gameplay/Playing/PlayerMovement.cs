using Horror.Inputs;
using UnityEngine;
using Mirror;

namespace Horror.Gameplay.Playing
{
	[RequireComponent(typeof(PlayerInputsListener), typeof(CharacterController))]
	public sealed class PlayerMovement : NetworkBehaviour, ICanMove
	{
		[Header("Movement")] 
		[SerializeField] private CharacterController _characterController;
		[SerializeField] private float _moveSpeed = 4.5f;
	
		[Range(0.0f, 0.5f)]
		[SerializeField] private float _moveSmothTime = 0.2f;
		
		private Vector2 _currentDirectionVelocity;
		private IInputService _inputService;
		private Vector2 _currentDirection;
		private Vector2 _movement;
		private float _velocityMultiplier = 1f;

		public void Initialize(IInputService inputService)
		{
			_inputService = inputService;

			_inputService.OnReadPlayerInputs += HandlePlayerInputs;
		}

		public void Dispose()
		{
			_inputService.OnReadPlayerInputs -= HandlePlayerInputs;
		}

		public void Tick(float deltaTime)
		{
			Transform playerTransform = transform; 
		
			Vector2 smoothDirection = GetSmoothDirection(_movement.normalized);
		
			Vector3 velocity = (playerTransform.right * smoothDirection.x) + (playerTransform.forward * smoothDirection.y);

			_characterController.Move(velocity * _moveSpeed * _velocityMultiplier * deltaTime);

			CmdUpdateMovementAnimation(smoothDirection);
		}

		[Command]
		private void CmdUpdateMovementAnimation(Vector2 smoothDirection)
		{
			RpcUpdateMovementAnimation(smoothDirection);
		}

		[ClientRpc]
		private void RpcUpdateMovementAnimation(Vector2 smoothDirection)
		{
			PlayerAnimationsController animationsController = GetComponentInChildren<PlayerAnimationsController>();
			
			animationsController.UpdateMovementAnimation(smoothDirection.x, smoothDirection.y);
		}
		
		private void HandlePlayerInputs(PlayerInputsData playerInputsData)
		{
			_movement = playerInputsData.PlayerMovement;
		}

		private Vector2 GetSmoothDirection(Vector2 targetDirection)
		{
			return _currentDirection = Vector2.SmoothDamp(_currentDirection, targetDirection, 
				ref _currentDirectionVelocity, _moveSmothTime);
		}

		public void SetVelocityMultiplier(float multiplier)
		{
			_velocityMultiplier = multiplier;
		}
		
		public bool IsMoving()
		{
			if (_movement.x != 0)
			{
				return true;
			}
            
			if (_movement.y != 0)
			{
				return true;
			}

			return false;
		}
	}
}
