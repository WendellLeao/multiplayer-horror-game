using UnityEngine;

namespace Horror.Inputs
{
	public struct PlayerInputsData
	{
		//Gameplay
		public Vector2 PlayerMovement;

		public bool PressPause;
		public bool PressInteract;
		public bool PressUseItem;
		public bool PressThrowObject;
		public bool IsSprinting;
		public bool IsCrouching;
		
		//UI
		public bool PressESC;
	}
}
