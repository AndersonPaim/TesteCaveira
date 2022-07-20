using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
    public class SoundEffect
    {
        public AudioMixerGroup Mixer;
        public AudioClip Clip;
        public float Volume;
        public bool Is3D;
    }
