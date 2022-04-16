using Horror.Pooling;

namespace Horror.Gameplay.Particles.SuperClass
{
	public sealed class ProjectileCollisionParticle : Particle
	{
		protected override void ReturnToPool()
		{
			// ObjectPooler.ReturnObjectToPool(PoolType.PROJECTILE_COLLISION_PARTICLES, this.gameObject);
		}
	}
}
