using Horror.Gameplay.VoiceRecognizer;
using Horror.Gameplay.Cameras;
using Horror.ServiceLocator;
using Horror.Inputs;
using UnityEngine;

namespace Horror.Gameplay.Playing
{
	public sealed class Player : NetworkEntity
	{
		[Header("Components")]
		[SerializeField] private PlayerInputsListener _playerInputsListener;
		[SerializeField] private PlayerMovement _playerMovement;
		[SerializeField] private PlayerRotation _playerRotation;
		[SerializeField] private PlayerSprint _playerSprint;
		[SerializeField] private PlayerCrouch _playerCrouch;
		[SerializeField] private PlayerView _playerView;
		[SerializeField] private Carrier _carrier;

		[Header("Camera")] 
		[SerializeField] private Transform _cameraTarget;
		
		private ICameraService _cameraService;
		private IInputService _inputService;
		private IVoiceService _voiceService;
		private Camera _mainCamera;
		private bool _isPlaying;

		public Transform CameraTarget => _cameraTarget;
		
		public void Begin(ICameraService cameraService, FirstPersonCamera firstPersonCamera)
		{
			BeginVoice();
			
			_inputService = GameServices.GetService<IInputService>();

			_cameraService = cameraService;
			_mainCamera = _cameraService.MainCamera;
			
			_playerView.Initialize();

			PlayerAnimationsController animationsController = _playerView.PlayerAnimationsController;
			
			_playerInputsListener.Initialize(_inputService);
			_playerMovement.Initialize(_inputService);
			_playerRotation.Initialize(_mainCamera);
			_playerSprint.Initialize(_inputService, animationsController);
			_playerCrouch.Initialize(_inputService, animationsController, firstPersonCamera);
			_carrier.Initialize(_cameraService, _inputService, _mainCamera);

			gameObject.name = "Horror Player [Local]";

			_isPlaying = true;
		}
		
		public void Stop()
		{
			_voiceService.Stop();
			
			_playerView.Dispose();
			
			_playerInputsListener.Dispose();
			_playerMovement.Dispose();
			_playerRotation.Dispose();
			_playerSprint.Dispose();
			_playerCrouch.Dispose();
			_carrier.Dispose();
			
			_isPlaying = false;
		}

		public void Tick(float deltaTime)
		{
			if (!_isPlaying)
			{
				return;
			}
			
			_playerView.Tick(deltaTime);
			
			_playerInputsListener.Tick(deltaTime);
			_playerMovement.Tick(deltaTime);
			_playerRotation.Tick(deltaTime);
			_playerSprint.Tick(deltaTime);
			_playerCrouch.Tick(deltaTime);
			_carrier.Tick(deltaTime);
		}

		private void BeginVoice()
		{
			_voiceService = GameServices.GetService<IVoiceService>();
			
			_voiceService.Begin();
		}
	}
}
