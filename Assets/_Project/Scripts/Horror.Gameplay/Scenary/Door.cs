using System.Collections;
using UnityEngine;

namespace Horror.Gameplay.Scenary
{
    public sealed class Door : Entity, IParanormalObject
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private float _delayInSeconds;
        
        private bool _isEvidence;

        public bool IsEvidence => _isEvidence;
        
        public void Close()
        {
            StartCoroutine(CloseCoroutine());
        }

        private IEnumerator CloseCoroutine()
        {
            yield return new WaitForSeconds(_delayInSeconds);
            
            _animator.SetTrigger("close_door");

            _isEvidence = true;
        }
    }
}
