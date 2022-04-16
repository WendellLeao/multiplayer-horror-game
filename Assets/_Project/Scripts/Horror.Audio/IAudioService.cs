using UnityEngine;

namespace Horror.Audio
{
    public interface IAudioService
    {
        void PlaySound(Sound sound, Vector3 position);
    }
}