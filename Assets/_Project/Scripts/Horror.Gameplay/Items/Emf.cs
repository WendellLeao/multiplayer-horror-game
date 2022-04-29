using Horror.Gameplay.Evidences;
using Horror.Gameplay.Scenary;
using UnityEngine;

namespace Horror.Gameplay.Items
{
    public sealed class Emf : Item
    {
        [Header("Emf")]
        [SerializeField] private float _detectionRange;
        
        private bool _isOn;

        public override void ExecuteAction()
        {
            _isOn = !_isOn;
        }

        protected override void OnTick(float deltaTime)
        {
            base.OnTick(deltaTime);

            if (!_isOn)
            {
                return;
            }

            CheckForParanormalInteractions();
        }

        private void CheckForParanormalInteractions()
        {
            RaycastHit hit;

            Transform mainCameraTransform = CameraService.MainCamera.transform;
            
            Ray ray = new Ray(mainCameraTransform.position, mainCameraTransform.forward);
            
            if (Physics.Raycast(ray, out hit, _detectionRange))
            {
                Transform selection = hit.transform;

                if (!selection.TryGetComponent(out IParanormalObject paranormalObject))
                {
                    return;
                }

                if (!EnemyHasEmfEvidence() || !paranormalObject.IsEvidence)
                {
                    return;
                }
                
                Debug.Log("Has EMF evidence!!");
            }
        }

        private bool EnemyHasEmfEvidence()
        {
            foreach (EvidenceData enemyEvidence in Enemy.Evidences)
            {
                if (enemyEvidence is EmfEvidenceData)
                {
                    return true;
                }
            }

            return false;
        }
    }
}