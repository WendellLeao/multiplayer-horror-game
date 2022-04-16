using UnityEngine;
using Mirror;

namespace Horror.Gameplay.Playing
{
	public sealed class PlayerRotation : NetworkBehaviour
	{
		[Header("Rotation")]
		[SerializeField] private float _rotationSpeed = 50f;
		[SerializeField] private float _spineRotationSpeed = 50f;
		[SerializeField] private Transform _spineTransform;
		
		private Camera _mainCamera;

		public void Initialize(Camera mainCamera)
		{
			_mainCamera = mainCamera;
		}

		public void Dispose()
		{ }
		
		public void Tick(float deltaTime)
		{
			RotateTowardsCameraDirection(_mainCamera, deltaTime);
			
			Vector3 mainCameraRotation = _mainCamera.transform.eulerAngles;
			
			CmdUpdateSpineRotation(mainCameraRotation);
		}

		private void RotateTowardsCameraDirection(Camera mainCamera, float deltaTime)
		{
			Transform cameraTransform = mainCamera.transform;
			
			Quaternion targetRotation = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0f);
			
			transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, _rotationSpeed * deltaTime);
		}

		[Command]
		private void CmdUpdateSpineRotation(Vector3 mainCameraRotation)
		{
			RpcUpdateSpineRotation(mainCameraRotation);
		}

		[ClientRpc]
		private void RpcUpdateSpineRotation(Vector3 mainCameraRotation)
		{
			Quaternion targetRotation = Quaternion.Euler(mainCameraRotation.x, mainCameraRotation.y, 0f);
			
			_spineTransform.rotation = Quaternion.Lerp(_spineTransform.rotation, targetRotation, _spineRotationSpeed);
		}
	}
}