using Horror.Gameplay.Evidences;
using UnityEngine;

namespace Horror.Gameplay.Items
{
    public sealed class Emf : Item
    {
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
            //Throw a ray and try to get paranormal interactions, if found throw log
            
            foreach (EvidenceData enemyEvidence in Enemy.Evidences)
            {
                if (enemyEvidence is EmfEvidenceData emfEvidenceData)
                {
                    Debug.Log("Has emf evidence, can turn red light and dispatch");

                    return;
                }
            }
            
            Debug.Log("Has no emf evidence");
        }
    }
}