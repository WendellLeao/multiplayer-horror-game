using Horror.ServiceLocator;
using Horror.Gameplay;
using Horror.Gameplay.Scenary;
using UnityEngine;

namespace Horror.Gameplay.VoiceRecognizer
{
    public sealed class VoiceListener : MonoBehaviour
    {
        [Header("Audio Source")]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _locationClip;
        [SerializeField] private AudioClip _locationClipPT;
        [SerializeField] private AudioClip _nameClip;
        [SerializeField] private AudioClip _nameClipPT;
        
        [Header("Phrases")]
        [SerializeField] private PhraseData askNamePhraseData;
        [SerializeField] private PhraseData askNamePhraseDataPt;
        [SerializeField] private PhraseData askLocationPhraseData;
        [SerializeField] private PhraseData askLocationPhraseDataPt;
        [SerializeField] private PhraseData giveSignPhraseData;
        [SerializeField] private PhraseData giveSignPhraseDataPt;

        [Header("Others")]
        [SerializeField] private Door _door;
        [SerializeField] private AudioSource _doorAudioSource;

        public void Begin()
        {
            IVoiceService voiceService = GameServices.GetService<IVoiceService>();

            voiceService.OnPhraseRecognized += HandlePhraseRecognized;
        }
        
        private void OnDisable()
        {
            IVoiceService voiceService = GameServices.GetService<IVoiceService>();

            voiceService.OnPhraseRecognized -= HandlePhraseRecognized;
        }
        
        private void HandlePhraseRecognized(PhraseData recognizedPhraseData)
        {
            if (recognizedPhraseData.ID == askNamePhraseData.ID)
            {
                _audioSource.clip = _nameClip;
                
                _audioSource.Play();
            }
            else if(recognizedPhraseData.ID == askLocationPhraseData.ID)
            {
                _audioSource.clip = _locationClip;
         
                _audioSource.Play();
            }
            if (recognizedPhraseData.ID == askNamePhraseDataPt.ID)
            {
                _audioSource.clip = _nameClipPT;
                
                _audioSource.Play();
            }
            else if(recognizedPhraseData.ID == askLocationPhraseDataPt.ID)
            {
                _audioSource.clip = _locationClipPT;
         
                _audioSource.Play();
            }
            else if(recognizedPhraseData.ID == giveSignPhraseData.ID || recognizedPhraseData.ID == giveSignPhraseDataPt.ID)
            {
                _door.Close();
                _doorAudioSource.Play();
            }
        }
    }
}
