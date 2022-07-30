using Interfaces;
using UnityEngine;
using UnityEngine.Audio;
using Event;
using Coimbra.Services.Events;

namespace Managers
{
    public class AudioManager : MonoBehaviour, IAudioPlayer
    {
        public static AudioManager sInstance;

        [SerializeField] private AudioMixer _gameAudioMixer;
        [SerializeField] private GameObject _audioPrefab;

        public void PlayAudio(SoundEffect soundEffect, Vector3 position)
        {
            AudioSource audioSource;

            GameObject obj = ObjectPooler.sInstance.SpawnFromPool(_audioPrefab.GetInstanceID());
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

        private void Awake()
        {
            if (sInstance != null)
            {
                DestroyImmediate(gameObject);
                return;
            }
            else
            {
                sInstance = this;
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
            SaveData data = SaveSystem.Load();
            _gameAudioMixer.SetFloat("EffectsVolume", Mathf.Log10(data.SoundfxVolume) * 20);
            _gameAudioMixer.SetFloat("MusicVolume", Mathf.Log10(data.MusicVolume) * 20);
        }

        private void SetupEvents()
        {
            OnMusicVolumeUpdate.AddListener(MusicVolume);
            OnEffectsVolumeUpdate.AddListener(EffectsVolume);
            OnPauseGame.AddListener(PauseAudio);
        }

        private void DestroyEvents()
        {
            OnMusicVolumeUpdate.RemoveAllListeners();
            OnEffectsVolumeUpdate.RemoveAllListeners();
            OnPauseGame.RemoveAllListeners();
        }

        private void PauseAudio(ref EventContext context, in OnPauseGame e)
        {
            if (e.IsPaused)
            {
                _gameAudioMixer.SetFloat("MasterVolume", -80);
            }
            else
            {
                _gameAudioMixer.SetFloat("MasterVolume", 0);
            }
        }

        private void EffectsVolume(ref EventContext context, in OnEffectsVolumeUpdate e)
        {
            _gameAudioMixer.SetFloat("EffectsVolume", Mathf.Log10(e.Volume) * 20);
            SaveData data = SaveSystem.localData;
            data.SoundfxVolume = e.Volume;
            SaveSystem.Save();
        }

        private void MusicVolume(ref EventContext context, in OnMusicVolumeUpdate e)
        {
            _gameAudioMixer.SetFloat("MusicVolume", Mathf.Log10(e.Volume) * 20);
            SaveData data = SaveSystem.localData;
            data.MusicVolume = e.Volume;
            SaveSystem.Save();
        }
    }
}