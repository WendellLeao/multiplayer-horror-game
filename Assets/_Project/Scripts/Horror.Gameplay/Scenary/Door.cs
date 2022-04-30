using System.Collections;
using UnityEngine;

namespace Horror.Gameplay.Scenary
{
    public sealed class Door : SceneryObject
    {
        [Header("Door")]
        [SerializeField] private Animator _animator;
        [SerializeField] private float _delayInSeconds;
        
        public override void Interact()
        {
            Close();
        }

        public void Close()
        {
            StartCoroutine(CloseCoroutine());
        }

        private IEnumerator CloseCoroutine()
        {
            yield return new WaitForSeconds(_delayInSeconds);
            
            _animator.SetTrigger("close_door");

            IsEvidence = true;

            yield return new WaitForSeconds(EvidenceDuration);
            
            IsEvidence = false;
        }
    }
}
