using UnityEngine;

namespace _Project.Scripts.Interfaces
{
    public interface IAudioPlayer
    {
        void PlayAudio(SoundEffect audio, Vector3 position);
    }
}