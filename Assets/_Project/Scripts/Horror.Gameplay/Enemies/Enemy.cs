using Horror.Gameplay.Enemies.EnemyAssemblies;
using Horror.Gameplay.VoiceRecognizer;
using Horror.Gameplay.Evidences;
using Horror.ServiceLocator;
using Horror.Events;
using UnityEngine;

namespace Horror.Gameplay.Enemies
{
    public abstract class Enemy : NetworkEntity, IHasEvidences
    {
        [SerializeField] private EnemyView _enemyView;
        [SerializeField] private PhraseData askLocationPhraseData;
        
        private EnemyAssemblyData _enemyAssemblyData;
        private EnemyData _enemyData;
        private IVoiceService _voiceService;

        public EvidenceData[] Evidences => _enemyData.Evidences;

        public void Begin(EnemyData enemyData, IVoiceService voiceService)
        {
            _enemyData = enemyData;
            _voiceService = voiceService;
            
            SetupEnemyAssemblyData(_enemyData);

            _enemyView.Setup(_enemyAssemblyData);
            
            SubscribeEvents();
            
            OnBegin();
        }

        public void Stop()
        {
            UnsubscribeEvents();

            OnStop();
        }

        protected virtual void OnBegin()
        { }
        
        protected virtual void OnStop()
        { }

        protected virtual void SubscribeEvents()
        {
            _voiceService.OnPhraseRecognized += HandlePhraseRecognized;
        }

        protected virtual void UnsubscribeEvents()
        {
            _voiceService.OnPhraseRecognized -= HandlePhraseRecognized;
        }

        protected virtual void HandlePhraseRecognized(PhraseData phraseData)
        {
            if (phraseData.ID != askLocationPhraseData.ID)//TODO: REMOVE THIS
            {
                return;
            }
            
            IEventService eventService = GameServices.GetService<IEventService>();//TODO: ...
            
            eventService.DispatchEvent(new EnemyResponseEvent());

            Debug.Log("Dispatch");
        }
        
        private void SetupEnemyAssemblyData(EnemyData enemyData)
        {
            EnemyAssemblyData[] enemyAssembly = enemyData.EnemyAssemblyDatas;
            
            int randomIndex = Random.Range(0, enemyAssembly.Length);

            _enemyAssemblyData = enemyAssembly[randomIndex];
        }
    }
}
