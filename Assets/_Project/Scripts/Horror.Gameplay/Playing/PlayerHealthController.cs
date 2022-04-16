using Horror.Gameplay.Health;
using UnityEngine;

namespace Horror.Gameplay.Playing
{
	public sealed class PlayerHealthController : MonoBehaviour, IDamageable
	{
		[Header("Health System")]
		[SerializeField] private int _maxHealthAmount = 100;
	
		[Header("Health System")]
		private HealthSystem _healthSystem;

		public void Initialize()
		{
			_healthSystem = new HealthSystem(_maxHealthAmount);
		}
		
		public void TakeDamage(int damageAmount)
		{
			// SoundManager.PlaySound(Sound.PLAYER_HITTED, transform.position);
		
			_healthSystem.Damage(damageAmount);

			RaiseOnHealthChangedEvent();

			// _playerGameEvents.OnPlayerIsDamaged?.Invoke();

			CheckIfPlayerDied();
		}

		public void AddHealth(int healthAmount)
		{
			_healthSystem.AddHealth(healthAmount);
		
			RaiseOnHealthChangedEvent();
		}

		private void RaiseOnHealthChangedEvent()
		{
			int currentHealthAmount = _healthSystem.GetCurrentHealthAmount();
			int maxHealthAmount = _healthSystem.GetMaxHealthAmount();
		
			// _playerGameEvents.OnHealthChanged?.Invoke(currentHealthAmount, maxHealthAmount);
		}
	
		private void CheckIfPlayerDied()
		{
			if(_healthSystem.GetCurrentHealthAmount() <= 0)
			{
				// _globalGameEvents.OnPlayerDied?.Invoke();
			}
		}
	}
}
