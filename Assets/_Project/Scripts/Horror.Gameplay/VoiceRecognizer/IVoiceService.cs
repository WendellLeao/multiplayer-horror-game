using System;

namespace Horror.Gameplay.VoiceRecognizer
{
    public interface IVoiceService
    {
        event Action<PhraseData> OnPhraseRecognized;
        void Begin();
        void Stop();
    }
}