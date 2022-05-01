using Horror.Gameplay.Enemies.Events;
using Horror.Gameplay.Evidences;
using Horror.Gameplay.Scenary;
using System.Threading.Tasks;
using Horror.Events;
using UnityEngine;
using Mirror;
using System;

namespace Horror.Gameplay.Items
{
    public sealed class Emf : Item
    {
        [Header("Emf")]
        [SerializeField] private EmfView _emfView;
        [SerializeField] private float _detectionRange;
        
        private bool _isDetectingParanormal;
        private bool _enemyHasManifested;
        
        [SyncVar]
        private bool _isOn;

        public override void ExecuteAction()
        {
            _isOn = !_isOn;

            if (!_isOn)
            {
                CmdTurnOff();

                return;
            }
            
            if (_enemyHasManifested)
            {
                CheckEvidenceAndTurnOn();
                    
                return;
            }
               
            CmdTurnOn((int) EmfScore.Idle);
        }

        protected override void OnBegin()
        {
            base.OnBegin();
            
            _emfView.Begin();
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            
            EventService.RemoveEventListener<EnemyResponseEvent>(HandleEnemyResponseEvent);
        }
        
        protected override void OnStop()
        {
            base.OnStop();
            
            _emfView.Stop();
        }

        protected override void OnTick(float deltaTime)
        {
            base.OnTick(deltaTime);
            
            if (!_enemyHasManifested)
            {
                _isDetectingParanormal = false;
            }
            
            if (!_isOn)//TODO: Check if is local player
            {
                return;
            }

            if (!_isDetectingParanormal)
            {
                CmdTurnOn((int) EmfScore.Idle);
            }
            
            CheckForParanormalInteractions();
        }

        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();
            
            EventService.AddEventListener<EnemyResponseEvent>(HandleEnemyResponseEvent);
        }

        private void CheckForParanormalInteractions()
        {
            Transform itemTransform = transform;
            
            Ray ray = new Ray(itemTransform.position, itemTransform.forward);
            
            if (Physics.Raycast(ray, out RaycastHit hit, _detectionRange))
            {
                IParanormalObject paranormalObject = hit.transform.GetComponentInParent<IParanormalObject>();
                
                if (paranormalObject == null)
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
            if (serviceEvent is EnemyResponseEvent enemyResponseEvent)
            {
                float manifestationDuration = enemyResponseEvent.ManifestationDuration;

                Debug.Log(manifestationDuration);
                
                SetEnemyHasManifestedAsync(manifestationDuration);

                _isDetectingParanormal = true;

                if (!_isOn)
                {
                    return;
                }

                CheckEvidenceAndTurnOn();
            }
        }
        
        private async void SetEnemyHasManifestedAsync(float manifestationDuration)
        {
            _enemyHasManifested = true;
            
            await Task.Delay(TimeSpan.FromSeconds(manifestationDuration));
            
            _enemyHasManifested = false;
            
            CmdTurnOff();
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
        
        private void CheckEvidenceAndTurnOn()
        {
            if (EnemyHasEmfEvidence())
            {
                CmdTurnOn((int) EmfScore.Evidence);

                return;
            }

            CmdTurnOn((int) EmfScore.EnemyManifestation);
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