using _Project.Scripts.Events;
using Coimbra.Services;
using Coimbra.Services.Events;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class SettingsScreen : MonoBehaviour
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private GameObject _settingsPopUp;
        [SerializeField] private Slider _effectsVolumeSlider;
        [SerializeField] private Slider _musicVolumeSlider;
        [SerializeField] private Slider _countdownSlider;
        [SerializeField] private TextMeshProUGUI _countdownValueText;

        private IEventService _eventService;

        private void Start()
        {
            SetupEvents();
            InitializeSliders();
        }

        private void OnEnable()
        {
            EntryAnimation();
        }

        private void OnDestroy()
        {
            DestroyEvents();
        }

        private void InitializeSliders()
        {
            SaveData data = SaveSystem.Load();
            _effectsVolumeSlider.value = data.SoundfxVolume;
            _musicVolumeSlider.value = data.MusicVolume;
            _countdownSlider.value = data.StartCountdown;
            _countdownValueText.text = data.StartCountdown.ToString() + " sec";
        }

        private void SetupEvents()
        {
            _eventService = ServiceLocator.Get<IEventService>();

            _closeButton.onClick.AddListener(CloseButtonClicked);
            _effectsVolumeSlider.onValueChanged.AddListener(ChangeEffectsVolume);
            _musicVolumeSlider.onValueChanged.AddListener(ChangeMusicVolume);
            _countdownSlider.onValueChanged.AddListener(ChangeStartCountdown);
        }

        private void DestroyEvents()
        {
            _closeButton.onClick.RemoveListener(CloseButtonClicked);
            _effectsVolumeSlider.onValueChanged.RemoveListener(ChangeEffectsVolume);
            _musicVolumeSlider.onValueChanged.RemoveListener(ChangeMusicVolume);
            _countdownSlider.onValueChanged.RemoveListener(ChangeStartCountdown);
        }

        private void EntryAnimation()
        {
            _settingsPopUp.transform.DOScale(1f, 0.2f).SetUpdate(true);
        }

        private void ExitAnimation()
        {
            _settingsPopUp.transform.DOScale(0, 0.2f).SetUpdate(true).OnComplete(CloseSettingsPopUp);
        }

        private void CloseSettingsPopUp()
        {
            gameObject.SetActive(false);
        }

        private void CloseButtonClicked()
        {
            ExitAnimation();
        }

        private void ChangeEffectsVolume(float volume)
        {
            OnEffectsVolumeUpdate volumeUpdate = new OnEffectsVolumeUpdate() { Volume = volume };
            volumeUpdate?.Invoke(_eventService);
        }

        private void ChangeMusicVolume(float volume)
        {
            OnMusicVolumeUpdate volumeUpdate = new OnMusicVolumeUpdate() { Volume = volume };
            volumeUpdate?.Invoke(_eventService);
        }

        private void ChangeStartCountdown(float time)
        {
            _countdownValueText.text = time + " sec";
            SaveData data = SaveSystem.LocalData;
            data.StartCountdown = (int)time;
            SaveSystem.Save();
        }
    }
}