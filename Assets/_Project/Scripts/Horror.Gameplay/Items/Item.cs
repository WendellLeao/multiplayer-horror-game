using Horror.Gameplay.Enemies;
using UnityEngine;
using Mirror;

namespace Horror.Gameplay.Items
{
    public abstract class Item : NetworkEntity, ICarriableObject
    {
        [Header("Item Components")] 
        [SerializeField] private Rigidbody _rigidBody;

        [Header("Throw")] 
        [SerializeField] private float _throwForce = 250f;
        [SerializeField] private float _throwHeight = 0.2f;

        private IHasEvidences _enemy;
        private Transform _container;
        private bool _hasInitialized;

        public bool CanBePickedUp => true;
        public bool HasInitialized => _hasInitialized;
        public NetworkIdentity NetIdentity => netIdentity;
        protected IHasEvidences Enemy => _enemy;
        
        public void Initialize(IHasEvidences enemy)
        {
            OnInitialize();

            _enemy = enemy;
            
            _hasInitialized = true;
        }
        
        public void Begin(Carrier carrier)
        {
            SubscribeEvents();

            OnBegin();
        }

        public void Dispose()
        {
            OnDispose();
        }
        
        public void Stop()
        {
            UnsubscribeEvents();
            
            OnStop();
        }
      
        public void Tick(float deltaTime)
        {
            OnTick(deltaTime);
        }

        public void SetContainer(Transform container)
        {
            if (container == null)
            {
                ExitContainer();
                
                return;
            }

            EnterContainer(container);
           
            OnContained();
        }
        
        public void Throw()
        {
            Stop();

            CmdThrow();
            
            OnThrow();
        }

        public abstract void ExecuteAction();

        protected virtual void SubscribeEvents()
        { }

        protected virtual void UnsubscribeEvents()
        { }
        
        protected virtual void OnInitialize()
        {}
        
        protected virtual void OnDispose()
        {}
        
        protected virtual void OnBegin()
        { }

        protected virtual void OnStop()
        { }

        protected virtual void OnTick(float deltaTime)
        { }

        protected virtual void OnContained()
        { }
        
        protected virtual void OnThrow()
        {}
        
        [Command(requiresAuthority = false)]
        private void CmdThrow()
        {
            RpcThrow();
        }

        [ClientRpc]
        private void RpcThrow()
        {
            AddForceToItem();

            SetContainer(null);
        }
        
        private void EnterContainer(Transform container)
        {
            _container = container;
            
            FreezeRigidbody();
            
            SetPositionAndRotation();
        }
        
        private void ExitContainer()
        {
            transform.SetParent(null);
                
            _container = null;
                
            UnfreezeRigidbody();
        }
        
        private void SetPositionAndRotation()
        {
            Transform itemTransform = transform;
            
            itemTransform.SetParent(_container);//TODO: FOLLOW THE CAMERA EACH FRAME

            itemTransform.position = _container.position;
            itemTransform.rotation = _container.rotation;
        }
        
        private void AddForceToItem()
        {
            Vector3 forward = transform.forward;

            Vector3 direction = new Vector3(forward.x, forward.y + _throwHeight, forward.z);

            _rigidBody.AddForce(direction * _throwForce);
        }
        
        private void FreezeRigidbody()
        {
            _rigidBody.useGravity = false;
            
            _rigidBody.velocity = Vector3.zero;
            _rigidBody.angularVelocity = Vector3.zero;
            
            _rigidBody.constraints = RigidbodyConstraints.FreezeAll;
        }
        
        private void UnfreezeRigidbody()
        {
            _rigidBody.useGravity = true;
            
            _rigidBody.constraints = RigidbodyConstraints.None;
        }
    }
}