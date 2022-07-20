using Interfaces;
using UnityEngine;
using UnityEngine.Audio;

namespace Managers
{
    public class AudioManager : MonoBehaviour, IAudioPlayer
    {
        [SerializeField] private GameManager _manager;
        [SerializeField] private AudioMixer _gameAudioMixer;

        private ObjectPooler _objectPooler;

        public void PlayAudio(SoundEffect soundEffect, Vector3 position)
        {
            AudioSource audioSource;

            GameObject obj = _objectPooler.SpawnFromPool(ObjectsTag.AudioSource);
            audioSource = obj.GetComponent<AudioSource>();
            obj.transform.position = position;

            audioSource.outputAudioMixerGroup = soundEffect.Mixer;
            audioSource.clip = soundEffect.Clip;
            audioSource.volume = soundEffect.Volume;
            obj.SetActive(false);
            obj.SetActive(true);

            if(soundEffect.Is3D)
            {
                audioSource.spatialBlend = 1;
            }
            else
            {
                audioSource.spatialBlend = 0;
            }

            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }

        private void Start()
        {
            Initialize();
            SetupEvents();
        }

        private void OnDestroy()
        {
            DestroyEvents();
        }

        private void Initialize()
        {
            _objectPooler = _manager.ObjectPooler;

            //SaveData data = SaveSystem.localData;
            //_gameAudioMixer.SetFloat(AudioMixerParameters.EffectsVolume, Mathf.Log10(data.soundfxVolume) * 20);
            //_gameAudioMixer.SetFloat(AudioMixerParameters.MusicVolume, Mathf.Log10(data.musicVolume) * 20);
        }

        /*private void EffectsVolume(float volume)
        {
            _gameAudioMixer.SetFloat(AudioMixerParameters.EffectsVolume, Mathf.Log10(volume) * 20);
            _finalAudioMixer.SetFloat(AudioMixerParameters.FinalVolume,  Mathf.Log10(volume) * 20);
        }*/

        private void PauseAudio(bool isPaused)
        {
            if (isPaused)
            {
                _gameAudioMixer.SetFloat("MasterVolume", -80);
            }
            else
            {
                _gameAudioMixer.SetFloat("MasterVolume", 0);
            }
        }

        private void SetupEvents()
        {
            _manager.OnPauseGame += PauseAudio;
        }

        private void DestroyEvents()
        {
            _manager.OnPauseGame -= PauseAudio;
        }
    }
}