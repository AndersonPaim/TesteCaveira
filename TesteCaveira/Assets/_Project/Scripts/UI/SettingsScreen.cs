using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SettingsScreen : MonoBehaviour
    {
        public delegate void SetVolumeHandler(float volume);
        public SetVolumeHandler OnSetEffectsVolume;

        [SerializeField] private Button _closeButton;
        [SerializeField] private GameObject _settingsPopUp;
        [SerializeField] private Slider _effectsVolumeSlider;

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
            _effectsVolumeSlider.value = data.soundfxVolume;
        }

        private void SetupEvents()
        {
            _closeButton.onClick.AddListener(CloseButtonClicked);
            _effectsVolumeSlider.onValueChanged.AddListener(ChangeEffectsVolume);
        }

        private void DestroyEvents()
        {
            _closeButton.onClick.RemoveListener(CloseButtonClicked);
            _effectsVolumeSlider.onValueChanged.RemoveListener(ChangeEffectsVolume);
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
            OnSetEffectsVolume?.Invoke(volume);
        }
    }
}