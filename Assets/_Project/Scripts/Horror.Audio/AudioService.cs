using System.Collections.Generic;
using Horror.ServiceLocator;
using Horror.Pooling;
using UnityEngine;

namespace Horror.Audio
{
    public sealed class AudioService : MonoBehaviour, IAudioService
    {
        [SerializeField] private AudioData[] _audioDatas;
        
        private Dictionary<Sound, AudioData> _audioDataDictionary;

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
        
        private void Awake()
        {
            _audioDataDictionary = new Dictionary<Sound, AudioData>();

            foreach (AudioData audioData in _audioDatas)
            {
                audioData.IsPlaying = false;
                
                _audioDataDictionary.Add(audioData.Sound, audioData);
            }
            
            GameServices.RegisterService<IAudioService>(this);
            
            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            GameServices.DeregisterService<IAudioService>();
        }
    }
}
