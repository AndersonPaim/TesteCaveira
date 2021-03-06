using System;
using DG.Tweening;
using Interfaces;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PauseScreen : MonoBehaviour
    {
        public Action OnResumeGame;

        [SerializeField] private GameManager _manager;
        [SerializeField] private GameObject _loadingScreen;
        [SerializeField] private GameObject _pausePopUp;
        [SerializeField] private GameObject _settingsScreen;
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _quitButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _settingsButton;

        private ISceneLoader _sceneLoader;

        private void Start()
        {
            StartEvents();
            Initialize();
        }

        private void OnEnable()
        {
            EntryAnimation();
        }

        private void OnDestroy()
        {
            DestroyEvents();
        }

        private void Initialize()
        {
            _sceneLoader = _manager.SceneController.GetComponent<ISceneLoader>();
        }

        private void StartEvents()
        {
            _resumeButton.onClick.AddListener(ResumeButtonClicked);
            _quitButton.onClick.AddListener(QuitButtonClicked);
            _restartButton.onClick.AddListener(RestartButtonClicked);
            _settingsButton.onClick.AddListener(SettingsButtonClicked);
        }

        private void DestroyEvents()
        {
            _resumeButton.onClick.RemoveListener(ResumeButtonClicked);
            _quitButton.onClick.RemoveListener(QuitButtonClicked);
            _restartButton.onClick.RemoveListener(RestartButtonClicked);
            _settingsButton.onClick.RemoveListener(SettingsButtonClicked);
        }

        private void EntryAnimation()
        {
            _pausePopUp.transform.DOScale(1, 0.2f).SetUpdate(true);
        }

        private void ExitAnimation()
        {
            _pausePopUp.transform.DOScale(0, 0.2f).SetUpdate(true).OnComplete(ClosePauseMenu);
        }

        private void ClosePauseMenu()
        {
            OnResumeGame?.Invoke();
            gameObject.SetActive(false);
        }

        private void ResumeButtonClicked()
        {
            ExitAnimation();
        }

        private void RestartButtonClicked()
        {
            _loadingScreen.SetActive(true);
            _sceneLoader.RestartScene();
        }

        private void QuitButtonClicked()
        {
            _loadingScreen.SetActive(true);
            _sceneLoader.LoadScene("MainMenu");
        }

        private void SettingsButtonClicked()
        {
            _settingsScreen.SetActive(true);
        }
    }
}
