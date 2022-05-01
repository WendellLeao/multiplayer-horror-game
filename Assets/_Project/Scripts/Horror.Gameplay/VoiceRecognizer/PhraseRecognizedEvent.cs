using Horror.Events;

namespace Horror.Gameplay.VoiceRecognizer
{
    public sealed class PhraseRecognizedEvent : ServiceEvent
    {
        public PhraseRecognizedEvent(PhraseData phraseData)
        {
            PhraseData = phraseData;
        }
        
        public PhraseData PhraseData { get; }
    }
}