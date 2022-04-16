using System.Collections.Generic;
using Horror.Gameplay.ObjectPooling;
using UnityEngine;

namespace Horror.Gameplay.Audio
{
	public static class SoundManager
	{
		private static AudioSourceProperties[] _audioSourceProperties;

		private static Dictionary<Sound, AudioSourceProperties> _audioSourcePropertiesDictionary;
    
		public static void Initialize(AudioSourceProperties[] audioSourcePropertiesArray)
		{
			_audioSourceProperties = audioSourcePropertiesArray;
		
			_audioSourcePropertiesDictionary = new Dictionary<Sound, AudioSourceProperties>();
        
			foreach (AudioSourceProperties audioSourceProperties in _audioSourceProperties)
			{
				_audioSourcePropertiesDictionary.Add(audioSourceProperties.Sound, audioSourceProperties);
			}
		}
	
		public static void PlaySound(Sound sound)
		{
			if (_audioSourcePropertiesDictionary.TryGetValue(sound, out AudioSourceProperties audioSourceProperties))
			{
				if (CanPlaySound(audioSourceProperties))
				{
					GetSoundPlayer().PlaySound(audioSourceProperties);
                
					audioSourceProperties.IsPlaying = true;
				}
			}
		}
	
		public static void PlaySound(Sound sound, Vector3 position)
		{
			if (_audioSourcePropertiesDictionary.TryGetValue(sound, out AudioSourceProperties audioSourceProperties))
			{
				if (CanPlaySound(audioSourceProperties))
				{
					GetSoundPlayer().PlaySound(audioSourceProperties, position);

					audioSourceProperties.IsPlaying = true;
				}
			}
		}
	
		private static bool CanPlaySound(AudioSourceProperties audioSourceProperties)
		{
			return !audioSourceProperties.PersistentSound || !audioSourceProperties.IsPlaying;
		}

		private static SoundPlayer GetSoundPlayer()
		{
			GameObject soundPlayerGameObject = ObjectPooler.GetObjectFromPool(PoolType.SOUND_PLAYER);
        
			SoundPlayer soundPlayer = soundPlayerGameObject.GetComponent<SoundPlayer>();

			return soundPlayer;
		}
	}
}
