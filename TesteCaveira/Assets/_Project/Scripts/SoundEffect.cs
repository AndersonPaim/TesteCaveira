using UnityEngine;
using UnityEngine.Audio;

namespace _Project.Scripts
{
    [System.Serializable]
    public class SoundEffect
    {
        public AudioMixerGroup Mixer;
        public AudioClip Clip;
        public float Volume;
        public bool Is3D;
    }
}
