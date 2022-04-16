using Horror.Gameplay.Health;
using Horror.Pooling;
using UnityEngine;

namespace Horror.Gameplay.Projectiles
{
	[RequireComponent(typeof(Rigidbody))]
	public abstract class Projectile : Entity
	{
		[Header("Projectile Components")]
		[SerializeField] protected Rigidbody _rigidbody;
	
		[SerializeField] protected BoxCollider _boxCollider;

		[SerializeField] protected Animator _animator;

		[Header("Fire")]
		[SerializeField] protected float _maxShootForce;

		[SerializeField] protected int _damageAmount;
	
		protected float _currentShootForce;

		[Header("Particles")]
		[SerializeField] protected GameObject _collisionParticlesObject;
	
		protected bool _canCollide, _canUnparent;
	
		public virtual void AddForce(Vector3 direction)
		{
			_rigidbody.AddForce(direction * _currentShootForce, ForceMode.Impulse);
		}
	
		public virtual void AddVelocity(Vector3 direction)
		{
			_rigidbody.velocity = direction * _currentShootForce;
		}
	
		public void ActiveBoxCollider()
		{
			_boxCollider.enabled = true;
		}

		public void DeactiveBoxCollider()
		{
			_boxCollider.enabled = false;
		}

		protected abstract void ReturnProjectileToPool();
    
		protected abstract void PlayCollisionSound();

		protected virtual void OnDisable()
		{
			ResetVelocity();
        
			ReturnProjectileToPool();
		
			SetCanCollide(true);
		}

		protected virtual void Update()
		{
		
		}

		protected virtual void LateUpdate()
		{
			if (CanUnparent)
			{
				Unparent();
			}
		}
	
		protected virtual void OnTriggerEnter(Collider other)
		{
			if (_canCollide)
			{
				DealsDamage(other.gameObject);
			
				PlayCollisionSound();
			
				PlayCollisionParticles();

				Deactivate();
			}
		}

		protected virtual void Initialize()
		{
			CanUnparent = true;

			SetCurrentShootForce(_maxShootForce);
		}
	
		protected virtual void DealsDamage(GameObject otherGameObject)
		{
			if (otherGameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
			{
				damageable.TakeDamage(_damageAmount);

				SetCanCollide(false);
			}
		}
	
		private void Awake()
		{
			Initialize();
		}
	
		private void Deactivate()
		{
			gameObject.SetActive(false);
		}

		private void Unparent()
		{
			transform.parent = null;

			CanUnparent = false;
		}

		private void PlayCollisionParticles()
		{
			// PoolType collisionPool = PoolType.PROJECTILE_COLLISION_PARTICLES;
			//
			// GameObject collisionParticlesObject = ObjectPooler.GetObjectFromPool(collisionPool);
			//
			// ParticleSystem collisionParticles = collisionParticlesObject.GetComponent<ParticleSystem>();
			//
			// collisionParticles.Play();
			//
			// collisionParticlesObject.transform.position = transform.position;
		}

		private void ResetVelocity()
		{
			_rigidbody.velocity = Vector3.zero;
		}

		public Rigidbody GetRigidbody()
		{
			return _rigidbody;
		}

		public float GetMaxShootForce()
		{
			return _maxShootForce;
		}

		public float GetCurrentShootForce()
		{
			return _currentShootForce;
		}
	
		public void SetPosition(Vector3 newPosition)
		{
			transform.position = newPosition;
		}
	
		public void SetRotation(Quaternion newRotation)
		{
			transform.rotation = newRotation;
		}

		public void SetParent(Transform newParent)
		{
			transform.parent = newParent;
		}

		public void SetCurrentShootForce(float newShootForce)
		{
			_currentShootForce = newShootForce;
		}

		public void SetDamageAmount(int newDamageAmount)
		{
			_damageAmount = newDamageAmount;
		}

		public void SetCanCollide(bool canCollide)
		{
			_canCollide = canCollide;
		}
	
		public bool CanUnparent
		{
			get => _canUnparent;
			set => _canUnparent = value;
		}
	}
}
