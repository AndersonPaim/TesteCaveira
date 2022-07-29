using Interfaces;
using UnityEngine;
using UnityEngine.Audio;
using Event;
using Coimbra.Services.Events;

namespace Managers
{
    public class AudioManager : MonoBehaviour, IAudioPlayer
    {
        [SerializeField] private GameManager _manager;
        [SerializeField] private AudioMixer _gameAudioMixer;
        [SerializeField] private GameObject _audioPrefab;

        private ObjectPooler _objectPooler;

        public void PlayAudio(SoundEffect soundEffect, Vector3 position)
        {
            AudioSource audioSource;

            GameObject obj = _objectPooler.SpawnFromPool(_audioPrefab.GetInstanceID());
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
            OnMusicVolumeUpdate.AddListener(MusicVolume);
            OnEffectsVolumeUpdate.AddListener(EffectsVolume);
            _manager.OnPauseGame += PauseAudio;
        }

        private void DestroyEvents()
        {
            OnMusicVolumeUpdate.RemoveAllListeners();
            OnEffectsVolumeUpdate.RemoveAllListeners();
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

        public void EffectsVolume(ref EventContext context, in OnEffectsVolumeUpdate volume)
        {
            _gameAudioMixer.SetFloat("EffectsVolume", Mathf.Log10(volume.Volume) * 20);
            SaveData data = SaveSystem.localData;
            data.SoundfxVolume = volume.Volume;
            SaveSystem.Save();
        }

        public void MusicVolume(ref EventContext context, in OnMusicVolumeUpdate volume)
        {
            _gameAudioMixer.SetFloat("MusicVolume", Mathf.Log10(volume.Volume) * 20);
            SaveData data = SaveSystem.localData;
            data.MusicVolume = volume.Volume;
            SaveSystem.Save();
        }
    }
}