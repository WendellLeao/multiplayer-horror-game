using Horror.Pooling;
using UnityEngine;

namespace Horror.Gameplay.Enemies.EnemyData
{
	[CreateAssetMenu(menuName = "Enemies/Enemy Data")]
	public sealed class EnemyData : ScriptableObject
	{
		[Header("Object Pooling")]
		public PoolType EnemyPool;
	
		[Header("Health System")]
		public int MaxHealthAmount;
	
		[Header("I.A")]
		public float RotationSpeed;
	
		[Header("Attacking")]
		public int AttackForce;

		public float AttackRate;

		[Header("Materials")]
		public Material[] Materials;
	
		[Header("Score")]
		public int ScoreAmountOnDeath;
	}
}
