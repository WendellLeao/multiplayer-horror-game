using Horror.Gameplay.Audio;
using Horror.Gameplay.ObjectPooling;
using UnityEngine;

namespace Horror.Gameplay.Projectiles
{
	public sealed class Arrow : Projectile
	{
		[Header("Trails")]
		[SerializeField] private GameObject _trailsGameObject;
	
		[Header("Animations")]
		private static readonly int _isBeingPulledID = Animator.StringToHash("isBeingPulled");

		private bool _isBeingPulled;

		public override void AddForce(Vector3 direction)
		{
			base.AddForce(direction);

			ActiveTrail();
		}
	
		public override void AddVelocity(Vector3 direction)
		{
			base.AddVelocity(direction);
		
			ActiveTrail();
		}
	
		protected override void OnDisable()
		{
			base.OnDisable();
		
			_trailsGameObject.SetActive(false);
		
			SetIsBeingPulled(false);
		}

		protected override void Update()
		{
			base.Update();
		
			_animator.SetBool(_isBeingPulledID, _isBeingPulled);
		}
	
		protected override void ReturnProjectileToPool()
		{
			ObjectPooler.ReturnObjectToPool(PoolType.ARROW_PROJECTILE, this.gameObject);
		}

		protected override void PlayCollisionSound()
		{
			SoundManager.PlaySound(Sound.ARROW_COLLISION, transform.position);
		}
	
		private void ActiveTrail()
		{
			if (_currentShootForce >= 10)
			{
				_trailsGameObject.SetActive(true);
			}
		}

		public void SetIsBeingPulled(bool isBeingPulled)
		{
			_isBeingPulled = isBeingPulled;
		}
	}
}
