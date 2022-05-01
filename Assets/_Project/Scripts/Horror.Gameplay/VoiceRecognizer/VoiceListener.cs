using Horror.Gameplay.Scenary;
using Horror.ServiceLocator;
using Horror.Audio;
using Horror.Events;
using UnityEngine;

namespace Horror.Gameplay.VoiceRecognizer
{
    public sealed class VoiceListener : MonoBehaviour
    {
        [Header("Phrases")]
        [SerializeField] private PhraseData askNamePhraseData;
        [SerializeField] private PhraseData askNamePhraseDataPt;
        [SerializeField] private PhraseData askLocationPhraseData;
        [SerializeField] private PhraseData askLocationPhraseDataPt;
        [SerializeField] private PhraseData giveSignPhraseData;
        [SerializeField] private PhraseData giveSignPhraseDataPt;

        [Header("Others")]
        [SerializeField] private Door _door;
        
        private IAudioService _audioService;
        private IEventService _eventService;

        public void Begin()
        {
            _audioService = GameServices.GetService<IAudioService>();
            
            _eventService = GameServices.GetService<IEventService>();
            
            _eventService.AddEventListener<PhraseRecognizedEvent>(HandlePhraseRecognized);
        }
        
        private void OnDisable()
        {
            _eventService.RemoveEventListener<PhraseRecognizedEvent>(HandlePhraseRecognized);
        }
        
        private void HandlePhraseRecognized(ServiceEvent serviceEvent)
        {
            if (serviceEvent is PhraseRecognizedEvent phraseRecognizedEvent)
            {
                PhraseData recognizedPhraseData = phraseRecognizedEvent.PhraseData;
                
                if (recognizedPhraseData.ID == askNamePhraseData.ID)
                {
                    _audioService.PlaySound(Sound.AgeResponseVoice, Vector3.zero);
                }
                else if(recognizedPhraseData.ID == askLocationPhraseData.ID)
                {
                    _audioService.PlaySound(Sound.LocationResponseVoice, Vector3.zero);
                }
                if (recognizedPhraseData.ID == askNamePhraseDataPt.ID)
                {
                    _audioService.PlaySound(Sound.AgeResponseVoicePt, Vector3.zero);
                }
                else if(recognizedPhraseData.ID == askLocationPhraseDataPt.ID)
                {
                    _audioService.PlaySound(Sound.LocationResponseVoicePt, Vector3.zero);
                }
                else if(recognizedPhraseData.ID == giveSignPhraseData.ID || recognizedPhraseData.ID == giveSignPhraseDataPt.ID)
                {
                    _door.Interact();
                
                    _audioService.PlaySound(Sound.CloseDoor, _door.transform.position);
                }
            }
        }
    }
}
