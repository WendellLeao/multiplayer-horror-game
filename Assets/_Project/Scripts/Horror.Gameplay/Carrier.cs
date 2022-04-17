using Horror.Gameplay.Cameras;
using Horror.Gameplay.Playing;
using Horror.Inputs;
using UnityEngine;
using Mirror;

namespace Horror.Gameplay
{
    public sealed class Carrier : NetworkBehaviour
    {
        [SerializeField] private Transform _itemContainer;
        [SerializeField] private float _pickUpRange = 0.5f;
        
        private ICarriableObject _containedCarriableObject;
        private Transform _mainCameraTransform;
        private ICameraService _cameraService;
        private IInputService _inputService;

        public void Initialize(ICameraService cameraService, IInputService inputService, Camera mainCamera)
        {
            _mainCameraTransform = mainCamera.transform;
            _cameraService = cameraService;
            _inputService = inputService;
            
            _inputService.OnReadPlayerInputs += HandleReadInputs;
        }
        
        public void Dispose()
        {
            _inputService.OnReadPlayerInputs -= HandleReadInputs;
        }

        public void Tick(float deltaTime)
        { }

        private void HandleReadInputs(PlayerInputsData playerInputsData)
        {
            if (_containedCarriableObject != null)
            {
                if (playerInputsData.PressThrowObject)
                {
                    _containedCarriableObject.Throw();
                
                    _containedCarriableObject = null;
                    
                    CmdSetOriginalAnimator();

                    return;
                }

                if (playerInputsData.PressUseItem)
                {
                    _containedCarriableObject.ExecuteAction();
                }
            }

            CheckForCarriableObjects(playerInputsData);
        }

        private void CheckForCarriableObjects(PlayerInputsData playerInputsData)
        {
            RaycastHit hit;
            
            Ray ray = new Ray(_mainCameraTransform.position, _mainCameraTransform.forward);
            
            if (Physics.Raycast(ray, out hit, _pickUpRange))
            {
                Transform selection = hit.transform;
                
                if (!playerInputsData.PressInteract)
                {
                    return;
                }
                
                if (selection.TryGetComponent(out ICarriableObject carriableObject))
                {
                    HandlePickUpItem(carriableObject);
                }
            }
        }

        private void HandlePickUpItem(ICarriableObject carriableObject)
        {
            if (!carriableObject.CanBePickedUp)
            {
                return;
            }
                  
            carriableObject.Begin(this);

            CmdPickUpItem(carriableObject.NetIdentity);

            _containedCarriableObject = carriableObject;
        }
        
        [Command]
        private void CmdPickUpItem(NetworkIdentity itemIdentity)
        {
            itemIdentity.RemoveClientAuthority();
            
            itemIdentity.AssignClientAuthority(connectionToClient);

            RpcPickUpItem(itemIdentity);
        }
        
        [ClientRpc]
        private void RpcPickUpItem(NetworkIdentity item)
        {
            SetCarrierAnimatorController();

            ICarriableObject carriableObject = item.GetComponent<ICarriableObject>();

            if (item.hasAuthority)
            {
                carriableObject.SetContainer(_cameraService.ItemContainer);

                return;
            }
            
            carriableObject.SetContainer(_itemContainer);
        }

        private void SetCarrierAnimatorController()
        {
            PlayerView playerView = GetComponentInChildren<PlayerView>();

            playerView.PlayerAnimationsController.SetCarrierAnimatorController();
        }

        [Command]
        private void CmdSetOriginalAnimator()
        {
            RpcSetOriginalAnimator();
        }
        
        [ClientRpc]
        private void RpcSetOriginalAnimator()
        {
            PlayerView playerView = GetComponentInChildren<PlayerView>();
            
            playerView.PlayerAnimationsController.SetOriginalAnimatorController();
        }
    }
}
