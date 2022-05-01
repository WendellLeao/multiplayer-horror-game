using Horror.Gameplay.Enemies.EnemyAssemblies;
using Horror.Gameplay.VoiceRecognizer;
using Horror.Gameplay.Enemies.Events;
using Horror.Gameplay.Evidences;
using Horror.ServiceLocator;
using Horror.Events;
using UnityEngine;

namespace Horror.Gameplay.Enemies
{
    public abstract class Enemy : NetworkEntity, IHasEvidences
    {
        [SerializeField] private EnemyView _enemyView;
        [SerializeField] private float _manifestationDuration = 3f;
        [SerializeField] private PhraseData askLocationPhraseData;//TODO: Remove this
        
        private EnemyAssemblyData _enemyAssemblyData;
        private IEventService _eventService;
        private EnemyData _enemyData;

        public EvidenceType[] Evidences => _enemyData.Evidences;

        public void Begin(EnemyData enemyData, IEventService eventService)
        {
            _enemyData = enemyData;
            _eventService = eventService;
            
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
            _eventService.AddEventListener<PhraseRecognizedEvent>(HandlePhraseRecognized);
        }

        protected virtual void UnsubscribeEvents()
        {
            _eventService.RemoveEventListener<PhraseRecognizedEvent>(HandlePhraseRecognized);
        }

        protected virtual void HandlePhraseRecognized(ServiceEvent serviceEvent)
        {
            if (serviceEvent is PhraseRecognizedEvent phraseRecognizedEvent)
            {
                PhraseData phraseData = phraseRecognizedEvent.PhraseData; 
                
                if (phraseData.ID != askLocationPhraseData.ID)//TODO: REMOVE THIS
                {
                    return;
                }
            
                _eventService.DispatchEvent(new EnemyResponseEvent(_manifestationDuration));
            }
        }
        
        private void SetupEnemyAssemblyData(EnemyData enemyData)
        {
            EnemyAssemblyData[] enemyAssembly = enemyData.EnemyAssemblyDatas;
            
            int randomIndex = Random.Range(0, enemyAssembly.Length);

            _enemyAssemblyData = enemyAssembly[randomIndex];
        }
    }
}
