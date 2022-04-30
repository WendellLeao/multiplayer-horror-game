using Horror.Gameplay.Evidences;
using Horror.Gameplay.Enemies;
using Horror.Gameplay.Scenary;
using Horror.Events;
using UnityEngine;
using Mirror;

namespace Horror.Gameplay.Items
{
    public sealed class Emf : Item
    {
        [Header("Emf")]
        [SerializeField] private EmfView _emfView;
        [SerializeField] private float _detectionRange;
        
        private bool _isDetectingParanormal;
        
        [SyncVar]
        private bool _isOn;

        public override void ExecuteAction()
        {
            _isOn = !_isOn;

            if (_isOn)
            {
                CmdTurnOn((int) EmfScore.Idle);

                return;
            }
            
            CmdTurnOff();
        }

        protected override void OnBegin()
        {
            base.OnBegin();
            
            _emfView.Begin();
        }

        protected override void OnStop()
        {
            base.OnStop();
            
            _emfView.Stop();
        }

        protected override void OnTick(float deltaTime)
        {
            base.OnTick(deltaTime);

            if (!_isOn ||! isLocalPlayer)
            {
                return;
            }

            CheckForParanormalInteractions();
            
            if (!_isDetectingParanormal)
            {
                CmdTurnOn((int) EmfScore.Idle);
            }
        }

        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();
            
            EventService.AddEventListener<EnemyResponseEvent>(HandleEnemyResponseEvent);
        }

        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();
            
            EventService.RemoveEventListener<EnemyResponseEvent>(HandleEnemyResponseEvent);
        }

        private void CheckForParanormalInteractions()
        {
            _isDetectingParanormal = false;
            
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

                if (!paranormalObject.IsEvidence)
                {
                    CmdTurnOn((int) EmfScore.Idle);
                    
                    return;
                }

                CheckAndGlowLight(paranormalObject);

                _isDetectingParanormal = true;
            }
        }

        private void CheckAndGlowLight(IParanormalObject paranormalObject)
        {
            switch (paranormalObject.EmfScore)
            {
                case EmfScore.Interaction:
                {
                    CmdTurnOn((int) EmfScore.Interaction);

                    break;
                }
                case EmfScore.ThrownObjects:
                {
                    CmdTurnOn((int) EmfScore.ThrownObjects);
                    
                    break;
                }
                case EmfScore.EnemyManifestation:
                {
                    CmdTurnOn((int) EmfScore.EnemyManifestation);
                   
                    break;
                }
                case EmfScore.Evidence:
                {
                    if (EnemyHasEmfEvidence())
                    {
                        CmdTurnOn((int) EmfScore.Evidence);
                        
                        break;
                    }
                    
                    CmdTurnOn((int) EmfScore.EnemyManifestation);
                    
                    break;
                }
            }
        }
        
        private void HandleEnemyResponseEvent(ServiceEvent serviceEvent)//TODO: IS NOT WORKING BECAUSE THE FLAG IS FALSE, CREATE A ASYNC METHOD
        {
            if (!_isOn)
            {
                return;
            }

            if (EnemyHasEmfEvidence())
            {
                CmdTurnOn((int) EmfScore.Evidence);
                        
                return;
            }
                    
            CmdTurnOn((int) EmfScore.EnemyManifestation);
        }

        [Command]
        private void CmdTurnOn(int emfScore)
        {
            RpcTurnOn(emfScore);
        }
        
        [ClientRpc]
        private void RpcTurnOn(int emfScore)
        {
            _emfView.TurnOn(emfScore);
        }
        
        [Command]
        private void CmdTurnOff()
        {
            RpcTurnOff();
        }

        [ClientRpc]
        private void RpcTurnOff()
        {
            _emfView.TurnOff();
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