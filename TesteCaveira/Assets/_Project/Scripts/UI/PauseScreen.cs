using System;
using Interfaces;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PauseScreen : MonoBehaviour
    {
        public Action OnPause;

        [SerializeField] private GameManager _manager;
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _quitButton;
        [SerializeField] private Button _restartButton;

        private ISceneLoader _sceneLoader;
        private bool _isPaused = false;

        private void Start()
        {
            StartEvents();
            Initialize();
        }

        private void OnDisable()
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
        }

        private void DestroyEvents()
        {
            _resumeButton.onClick.RemoveListener(ResumeButtonClicked);
            _quitButton.onClick.RemoveListener(QuitButtonClicked);
            _restartButton.onClick.RemoveListener(RestartButtonClicked);
        }

        private void ResumeButtonClicked()
        {
            OnPause?.Invoke();
            _isPaused = false;
            gameObject.SetActive(false);
        }

        private void RestartButtonClicked()
        {
            _sceneLoader.RestartScene();
        }

        private void QuitButtonClicked()
        {
            _sceneLoader.LoadScene("MainMenu");
        }
    }
}