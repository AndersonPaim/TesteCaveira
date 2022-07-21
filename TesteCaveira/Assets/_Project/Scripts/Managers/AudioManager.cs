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

            SaveData data = SaveSystem.Load();
            _gameAudioMixer.SetFloat("EffectsVolume", Mathf.Log10(data.SoundfxVolume) * 20);
            _gameAudioMixer.SetFloat("MusicVolume", Mathf.Log10(data.MusicVolume) * 20);
        }

        private void SetupEvents()
        {
            _manager.SettingsScreen.OnSetEffectsVolume += EffectsVolume;
            _manager.SettingsScreen.OnSetMusicVolume += MusicVolume;
            _manager.OnPauseGame += PauseAudio;
        }

        private void DestroyEvents()
        {
            _manager.SettingsScreen.OnSetEffectsVolume -= EffectsVolume;
            _manager.SettingsScreen.OnSetMusicVolume -= MusicVolume;
            _manager.OnPauseGame -= PauseAudio;
        }

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

        private void EffectsVolume(float volume)
        {
            _gameAudioMixer.SetFloat("EffectsVolume", Mathf.Log10(volume) * 20);
            SaveData data = SaveSystem.localData;
            data.SoundfxVolume = volume;
            SaveSystem.Save();
        }

        private void MusicVolume(float volume)
        {
            _gameAudioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
            SaveData data = SaveSystem.localData;
            data.MusicVolume = volume;
            SaveSystem.Save();
        }
    }
}