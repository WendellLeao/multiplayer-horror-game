using UnityEngine.InputSystem;
using UnityEngine;
using Mirror;

namespace Horror.Inputs
{
	public sealed class PlayerInputsListener : NetworkBehaviour
	{
		[Header("Input System")]
		private PlayerInputs _playerInputs;

		private PlayerInputs.LandControlsActions _playerLandControls;
		private PlayerInputs.UIControlsActions _uiControls;
		private PlayerInputsData _playerInputsData;
		private IInputService _inputService;
		
		[Header("Inputs Data")]
		//Gameplay
		private Vector2 _playerMovement;
		private Vector2 _mousePosition;
		private Vector2 _mouseLook;
		private bool _pressUseItem;
		private bool _pressInteract;
		private bool _pressThrowObject;
		private bool _isSprinting;
		private bool _isCrouching;
		
		//UI
		private bool _pressESC;

		public void Initialize(IInputService inputService)
		{
			_inputService = inputService;
			
			InitializePlayerInputs();

			EnablePlayerInputs();

			SubscribeEvents();
		}

		public void Dispose()
		{
			DisablePlayerInputs();
			
			UnsubscribeEvents();
		}
		
		public void Tick(float deltaTime)
		{
			UpdatePlayerInputsData();
			
			_inputService.DispatchPlayerInputs(_playerInputsData);
		
			ResetInputs();
		}
	
		private void SubscribeEvents()
		{
			//GAMEPLAY
			_playerLandControls.Movement.performed += SetPlayerMovement;
			
			_playerLandControls.Sprint.performed += HandleSprint;
			_playerLandControls.Sprint.canceled += HandleSprint;
			
			_playerLandControls.Crouch.performed += HandleCrouch;
			_playerLandControls.Crouch.canceled += HandleCrouch;
			
			_playerLandControls.UseItem.performed += HandleUseItem;
			_playerLandControls.UseItem.canceled += HandleUseItem;
		
			_playerLandControls.Interact.performed += HandleInteract;
			_playerLandControls.Interact.canceled += HandleInteract;
			
			_playerLandControls.ThrowObject.performed += HandleThrowObject;
			_playerLandControls.ThrowObject.canceled += HandleThrowObject;
			
			//UI
			_uiControls.PressESC.performed += HandleESC;
			_uiControls.PressESC.canceled += HandleESC;
		}

		private void UnsubscribeEvents()
		{
			//GAMEPLAY
			_playerLandControls.Movement.performed -= SetPlayerMovement;
			
			_playerLandControls.Sprint.performed -= HandleSprint;
			_playerLandControls.Sprint.canceled -= HandleSprint;
			
			_playerLandControls.Crouch.performed -= HandleCrouch;
			_playerLandControls.Crouch.canceled -= HandleCrouch;
			
			_playerLandControls.UseItem.performed -= HandleUseItem;
			_playerLandControls.UseItem.canceled -= HandleUseItem;
			
			_playerLandControls.Interact.performed -= HandleInteract;
			_playerLandControls.Interact.canceled -= HandleInteract;
			
			//UI
			_uiControls.PressESC.performed -= HandleESC;
			_uiControls.PressESC.canceled -= HandleESC;
		}

		private void InitializePlayerInputs()
		{
			_playerInputs = new PlayerInputs();
		
			_playerLandControls = _playerInputs.LandControls;

			_uiControls = _playerInputs.UIControls;
		}
	
		private void EnablePlayerInputs()
		{
			_playerInputs.Enable();
		}
	
		private void DisablePlayerInputs()
		{
			_playerInputs.Disable();
		}

		private void ResetInputs()
		{
			_pressUseItem = false;
			_pressInteract = false;
			_isCrouching = false;
			_pressESC = false;
		}

		private void UpdatePlayerInputsData()
		{
			//Gameplay
			_playerInputsData.PlayerMovement = _playerMovement;
			_playerInputsData.PressInteract = _pressInteract;
			_playerInputsData.PressUseItem = _pressUseItem;
			_playerInputsData.PressThrowObject = _pressThrowObject;
			_playerInputsData.IsSprinting = _isSprinting;
			_playerInputsData.IsCrouching = _isCrouching;
			
			//UI
			_playerInputsData.PressESC = _pressESC;
		}
		
		private void HandleSprint(InputAction.CallbackContext context)
		{
			switch(context.phase)
			{
				case InputActionPhase.Performed:
				{
					_isSprinting = true;
					break;
				}
				case InputActionPhase.Canceled:
				{
					_isSprinting = false;
					break;
				}
			}
		}
		
		private void HandleUseItem(InputAction.CallbackContext context)
		{
			switch(context.phase)
			{
				case InputActionPhase.Performed:
				{
					_pressUseItem = true;
					break;
				}
				case InputActionPhase.Canceled:
				{
					_pressUseItem = false;
					break;
				}
			}
		}
		
		private void HandleInteract(InputAction.CallbackContext context)
		{
			switch(context.phase)
			{
				case InputActionPhase.Performed:
				{
					_pressInteract = true;
					break;
				}
				case InputActionPhase.Canceled:
				{
					_pressInteract = false;
					break;
				}
			}
		}
		
		private void HandleThrowObject(InputAction.CallbackContext context)
		{
			switch(context.phase)
			{
				case InputActionPhase.Performed:
				{
					_pressThrowObject = true;
					break;
				}
				case InputActionPhase.Canceled:
				{
					_pressThrowObject = false;
					break;
				}
			}
		}
		
		private void HandleCrouch(InputAction.CallbackContext context)
		{
			switch(context.phase)
			{
				case InputActionPhase.Performed:
				{
					_isCrouching = true;
					break;
				}
				case InputActionPhase.Canceled:
				{
					_isCrouching = false;
					break;
				}
			}
		}
		
		private void HandleESC(InputAction.CallbackContext context)
		{
			switch(context.phase)
			{
				case InputActionPhase.Performed:
				{
					_pressESC = true;
					break;
				}
				case InputActionPhase.Canceled:
				{
					_pressESC = false;
					break;
				}
			}
		}

		private void SetPlayerMovement(InputAction.CallbackContext action)
		{
			_playerMovement = action.ReadValue<Vector2>();
		}
	}
}
