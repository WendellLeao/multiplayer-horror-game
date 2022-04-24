using System.Collections.Generic;
using Horror.ServiceLocator;
using Horror.Pooling;
using UnityEngine;

namespace Horror.Audio
{
    public sealed class AudioService : IAudioService
    {
        private const string AudioDatasPath = "GameServices/AudioService/AudioDatas";
        
        private Dictionary<Sound, AudioData> _audioDataDictionary;
        private AudioData[] _audioDatas;

        public AudioService()
        {
            _audioDatas = Resources.LoadAll<AudioData>(AudioDatasPath);
            
            _audioDataDictionary = new Dictionary<Sound, AudioData>();

            foreach (AudioData audioData in _audioDatas)
            {
                audioData.IsPlaying = false;
                
                _audioDataDictionary.Add(audioData.Sound, audioData);
            }
        }

        public void PlaySound(Sound sound, Vector3 position)
        {
            if (_audioDataDictionary.TryGetValue(sound, out AudioData audioData))
            {
                if (!CanPlaySound(audioData))
                {
                    return;
                }

                SoundPlayer soundPlayer = GetSoundPlayerFromPool();
                
                soundPlayer.PlaySound(audioData, position);

                audioData.IsPlaying = true;
            }
        }

        private bool CanPlaySound(AudioData audioData)
        {
            if (!audioData.PersistentSound)
            {
                return true;
            }

            if (!audioData.IsPlaying)
            {
                return true;
            }

            return false;
        }

        private SoundPlayer GetSoundPlayerFromPool()
        {
            IPoolingService poolingService = GameServices.GetService<IPoolingService>();
            
            GameObject soundPlayerGameObject = poolingService.GetObjectFromPool(PoolType.SoundPlayer);

            SoundPlayer soundPlayer = soundPlayerGameObject.GetComponent<SoundPlayer>();

            return soundPlayer;
        }
    }
}
