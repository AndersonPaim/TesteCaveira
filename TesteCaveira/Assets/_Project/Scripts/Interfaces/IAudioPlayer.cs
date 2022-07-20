using UnityEngine;

namespace Interfaces
{
    public interface IAudioPlayer
    {
        void PlayAudio(SoundEffect audio, Vector3 position);
    }
}