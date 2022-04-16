using UnityEngine;

namespace Horror.Gameplay
{
    public sealed class MovementStatusEffect : MonoBehaviour
    {
        private ICanMove _movement;

        public void SetVelocityMultiplier(float multiplier)
        {
            _movement.SetVelocityMultiplier(multiplier);
        }
        
        public void ResetVelocityMultiplier()
        {
            _movement.SetVelocityMultiplier(1f);
        }
        
        private void Awake()
        {
            _movement = GetComponent<ICanMove>();
        }
    }
}
