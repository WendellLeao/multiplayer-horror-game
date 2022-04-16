using Horror.ServiceLocator;
using System.Collections;
using Horror.Pooling;
using UnityEngine;

namespace Horror.Audio
{
    public sealed class SoundPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;

        private AudioData _audioData;

        public void PlaySound(AudioData audioData, Vector3 position)
        {
            SetAndPlayAudioSource(audioData);

            transform.position = position;
        }

        private void OnDisable()
        {
            IPoolingService poolingService = GameServices.GetService<IPoolingService>();

            if (poolingService == null)
            {
                Debug.Log("dsds");
                
                return;
            }
            
            poolingService.ReturnObjectToPool(PoolType.SoundPlayer, gameObject);

            if (_audioData != null)
            {
                _audioData.IsPlaying = false;
            }
        }

        private void SetAndPlayAudioSource(AudioData audioData)
        {
            SetAudioData(audioData);

            _audioSource.Play();

            if (!_audioSource.loop)
            {
                StartCoroutine(DeactivateSoundGameObject());
            }
        }

        private IEnumerator DeactivateSoundGameObject()
        {
            yield return new WaitForSeconds(_audioSource.clip.length);

            gameObject.SetActive(false);
        }

        private void SetAudioData(AudioData audioData)
        {
            _audioData = audioData;

            int randomIndex = Random.Range(0, audioData.AudioClips.Length);
            _audioSource.clip = audioData.AudioClips[randomIndex];

            _audioSource.volume = audioData.Volume;

            _audioSource.pitch = audioData.Pitch;

            _audioSource.spatialBlend = audioData.SpatialBlend;

            _audioSource.loop = audioData.Loop;

            _audioSource.outputAudioMixerGroup = audioData.AudioMixerGroup;

            if (audioData.PersistentSound)
            {
                DontDestroyOnLoad(_audioSource.gameObject);
            }
        }
    }
}