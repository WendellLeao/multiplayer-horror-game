using System.Collections;
using Horror.Gameplay.ObjectPooling;
using UnityEngine;

namespace Horror.Gameplay.Audio
{
	public sealed class SoundPlayer : MonoBehaviour
	{
		[SerializeField] private AudioSource _audioSource;

		private AudioSourceProperties _audioSourceProperties;

		public void PlaySound(AudioSourceProperties audioSourceProperties)
		{
			SetAndPlayAudioSource(audioSourceProperties);
		}
	
		public void PlaySound(AudioSourceProperties audioSourceProperties, Vector3 position)
		{
			SetAndPlayAudioSource(audioSourceProperties);
        
			transform.position = position;
		}

		private void OnDisable()
		{
			ObjectPooler.ReturnObjectToPool(PoolType.SOUND_PLAYER, this.gameObject);

			if (_audioSourceProperties != null)
			{
				_audioSourceProperties.IsPlaying = false;
			}
		}
    
		private void SetAndPlayAudioSource(AudioSourceProperties audioSourceProperties)
		{
			SetAudioSourceProperties(audioSourceProperties);

			_audioSource.Play();
        
			if (!_audioSource.loop)
			{
				StartCoroutine(DeactivateSoundGameObject());
			}
		}
    
		private IEnumerator DeactivateSoundGameObject()
		{
			yield return new WaitForSeconds(_audioSource.clip.length);

			this.gameObject.SetActive(false);
		}

		private void SetAudioSourceProperties(AudioSourceProperties audioSourceProperties)
		{
			_audioSourceProperties = audioSourceProperties;
        
			int randomIndex = Random.Range(0, audioSourceProperties.AudioClips.Length);
			_audioSource.clip = audioSourceProperties.AudioClips[randomIndex];
        
			_audioSource.volume = audioSourceProperties.Volume;

			_audioSource.pitch = audioSourceProperties.Pitch;
		
			_audioSource.spatialBlend = audioSourceProperties.SpatialBlend;
        
			_audioSource.loop = audioSourceProperties.Loop;
        
			_audioSource.outputAudioMixerGroup = audioSourceProperties.AudioMixerGroup;

			if (audioSourceProperties.PersistentSound)
			{
				DontDestroyOnLoad(_audioSource.gameObject);
			}
		}
	}
}
