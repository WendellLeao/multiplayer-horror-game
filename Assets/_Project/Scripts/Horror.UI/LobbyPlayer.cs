using UnityEngine.InputSystem.Controls;
using Horror.Gameplay.Playing;
using UnityEngine.InputSystem;
using Horror.ServiceLocator;
using Horror.Inputs;
using UnityEngine;
using Mirror;
using TMPro;

namespace Horror.UI.Lobby
{
    public sealed class LobbyPlayer : NetworkBehaviour
    {
        [Header("UI")]
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _readinessText;
        [SerializeField] private Color _readyTextColor;
        [SerializeField] private Color _unreadyTextColor;

        [Header("Spine")] 
        [SerializeField] private PlayerInputsListener _playerInputsListener;
        [SerializeField] private Transform _spineTransform;
        [SerializeField] private float _spineRotationSpeed = 0.3f;
        
        private const string UnreadyText = "Unready";
        private const string ReadyText = "Ready";
        
        [SyncVar(hook = nameof(UpdatePlayerName))] 
        private string _playerName;
        [SyncVar(hook = nameof(UpdateReadiness))] 
        private bool _isReady;

        public void Initialize(string playerName, bool isReady)
        {
            CmdUpdatePlayerName(playerName);
            CmdUpdateReadiness(isReady);

            IInputService inputService = GameServices.GetService<IInputService>();
            
            _playerInputsListener.Initialize(inputService);

            inputService.OnReadPlayerInputs += HandlePlayerInputs;
        }

        public void Dispose()
        {
            _playerInputsListener.Dispose();

            IInputService inputService = GameServices.GetService<IInputService>();

            inputService.OnReadPlayerInputs -= HandlePlayerInputs;
        }

        public void Tick(float deltaTime)
        {
            if (!NetworkServer.active)
            {
                return;
            }

            _playerInputsListener.Tick(deltaTime);
        }

        private void HandlePlayerInputs(PlayerInputsData playerInputsData)
        {
            Vector2Control mouseCurrentPosition = Mouse.current.position;

            CmdUpdateSpineRotation(mouseCurrentPosition.ReadValue());
        }

        [Command]
        private void CmdUpdatePlayerName(string playerName)
        {
            RpcUpdatePlayerName(playerName);
        }

        [ClientRpc]
        private void RpcUpdatePlayerName(string playerName)
        {
            _nameText.text = playerName;

            _playerName = playerName;
        }
        
        [Command(requiresAuthority = false)]
        public void CmdUpdateReadiness(bool isReady)
        {
            RpcUpdateReadiness(isReady);
        }

        [ClientRpc]
        private void RpcUpdateReadiness(bool isReady)
        {
            _isReady = isReady;
            
            if (isReady)
            {
                _readinessText.text = ReadyText;
                _readinessText.color = _readyTextColor;
    
                return;
            }
            
            _readinessText.text = UnreadyText;
            _readinessText.color = _unreadyTextColor;
        }

        private void UpdatePlayerName(string oldPlayerName, string newPlayerName)
        {
            _nameText.text = newPlayerName;
        }

        private void UpdateReadiness(bool oldValue, bool isReady)
        {
            CmdUpdateReadiness(isReady);
        }
        
        [Command]
        private void CmdUpdateSpineRotation(Vector2 mousePosition)
        {
            RpcUpdateSpineRotation(mousePosition);
        }

        [ClientRpc]
        private void RpcUpdateSpineRotation(Vector3 mousePosition)
        {
            Camera mainCamera = Camera.main;
            
            Ray mouseRay = mainCamera.ScreenPointToRay(mousePosition);

            float midPoint = (_spineTransform.position - mainCamera.transform.position).magnitude * _spineRotationSpeed;

            _spineTransform.LookAt(mouseRay.origin + mouseRay.direction * midPoint);
        }
    }
}