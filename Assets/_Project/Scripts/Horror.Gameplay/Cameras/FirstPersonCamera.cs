using Cinemachine;
using UnityEngine;

namespace Horror.Gameplay.Cameras
{
	public sealed class FirstPersonCamera : MonoBehaviour
	{
		[Header("Virtual Camera")]
		[SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;

		public Transform CameraTarget => _cinemachineVirtualCamera.Follow;
		
		public void Initialize(Transform target)
		{
			_cinemachineVirtualCamera.enabled = true;
			
			SetTarget(target);
		}

		public void SetTarget(Transform target)
		{
			_cinemachineVirtualCamera.Follow = target;
			_cinemachineVirtualCamera.LookAt = target;
		}
	}
}
