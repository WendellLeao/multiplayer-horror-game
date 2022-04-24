using UnityEngine.Windows.Speech;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Horror.Gameplay.VoiceRecognizer
{
    public sealed class VoiceService : IVoiceService
    {
        public event Action<PhraseData> OnPhraseRecognized;
        
        private const string PhraseDatasPath = "GameServices/VoiceService/PhraseDatas";

        private List<string> _phrases = new List<string>();
        private PhraseRecognizer _phraseRecognizer;
        private PhraseData[] _phraseDatas;
        private AudioClip _audioClip;
        private bool _hasBegun;

        public VoiceService()
        {
            _phraseDatas = Resources.LoadAll<PhraseData>(PhraseDatasPath);
        }
        
        public void Begin()
        {
            if (_hasBegun)
            {
                Stop();
            }
            
            StartMicrophone();
            
            if (_phraseDatas.Length <= 0)
            {
                return;
            }

            BeginPhraseRecognizer();

            _hasBegun = true;
        }

        public void Stop()
        {
            if (_phraseRecognizer == null)
            {
                return;
            }

            if (!_phraseRecognizer.IsRunning)
            {
                return;
            }
            
            _phraseRecognizer.OnPhraseRecognized -= HandlePhraseRecognized;
                
            _phraseRecognizer.Stop();
                
            _phraseRecognizer.Dispose();
            
            _hasBegun = false;
        }
        
        private void HandlePhraseRecognized(PhraseRecognizedEventArgs args)
        {
            string word = args.text;
            
            Debug.Log("You said: <b>" + word + "</b>");

            foreach (PhraseData phraseData in _phraseDatas)
            {
                foreach (string phrase in phraseData.Phrases)
                {
                    if (phrase != word)
                    {
                        continue;
                    }
                
                    OnPhraseRecognized?.Invoke(phraseData);

                    return;
                }
            }
        }

        private void BeginPhraseRecognizer()
        {
            string[] phrases = ConvertToStringArray(_phraseDatas);
            
            _phraseRecognizer = new KeywordRecognizer(phrases, ConfidenceLevel.Medium);//TODO: ADJUST THIS
            
            _phraseRecognizer.OnPhraseRecognized += HandlePhraseRecognized;
                
            _phraseRecognizer.Start();
        }
        
        private void StartMicrophone()
        {
            string[] devices = Microphone.devices;

            if (devices.Length <= 0)
            {
                Debug.Log("No device connected");

                return;
            }
            
            foreach (string device in devices)
            {
                Debug.Log("Connected device: " + device);

                AudioClip microphoneClip = Microphone.Start(device, true, 10, 44100);

                _audioClip = microphoneClip;
                _audioClip.name = device;
            }
        }
        
        private string[] ConvertToStringArray(IReadOnlyList<PhraseData> phrasesToConvert)
        {
            _phrases.Clear();
        
            for (int i = 0; i < phrasesToConvert.Count; i++)
            {
                PhraseData phraseData = phrasesToConvert[i];

                string[] phrases = phraseData.Phrases;
                
                for (int j = 0; j < phrases.Length; j++)
                {
                    string phrase = phrases[j];
                    
                    _phrases.Add(phrase);
                }
            }

            return _phrases.ToArray();
        }
    }
}
