using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SettingsScreen : MonoBehaviour
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private GameObject _settingsPopUp;

        private void Start()
        {
            SetupEvents();
        }

        private void OnEnable()
        {
            EntryAnimation();
        }

        private void OnDestroy()
        {
            DestroyEvents();
        }

        private void SetupEvents()
        {
            _closeButton.onClick.AddListener(CloseButtonClicked);
        }

        private void DestroyEvents()
        {
            _closeButton.onClick.RemoveListener(CloseButtonClicked);
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
    }
}