using UnityEngine;

namespace Horror.Gameplay.VoiceRecognizer
{
    [CreateAssetMenu(menuName = "VoiceService/PhraseData", fileName = "NewPhraseData")]
    public sealed class PhraseData : ScriptableObject
    {
        public string ID;
        public string[] Phrases;
    }
}