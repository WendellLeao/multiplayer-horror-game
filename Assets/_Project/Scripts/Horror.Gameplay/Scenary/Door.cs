using System.Threading.Tasks;
using UnityEngine;
using System;

namespace Horror.Gameplay.Scenary
{
    public sealed class Door : SceneryObject
    {
        [Header("Door")]
        [SerializeField] private Animator _animator;
        [SerializeField] private float _delayInSeconds;
        
        private static readonly int CloseDoorHash = Animator.StringToHash("CloseDoor");

        public override void Interact()
        {
            CloseAsync();
        }

        private async void CloseAsync()
        {
            await Task.Delay(TimeSpan.FromSeconds(_delayInSeconds));
            
            _animator.SetTrigger(CloseDoorHash);

            IsEvidence = true;

            await Task.Delay(TimeSpan.FromSeconds(_delayInSeconds));
            
            IsEvidence = false;
        }
    }
}
