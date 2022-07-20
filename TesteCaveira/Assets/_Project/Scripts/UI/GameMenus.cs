using System;
using Interfaces;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameMenus : MonoBehaviour
    {
        public Action OnPause;

        [SerializeField] private GameManager _manager;
        [SerializeField] private GameObject _pauseMenu;
        [SerializeField] private GameObject _victoryMenu;
        [SerializeField] private GameObject _defeatedMenu;
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _victoryQuitButton;
        [SerializeField] private Button _victoryRestartButton;
        [SerializeField] private Button _defeatedQuitButton;
        [SerializeField] private Button _defeatedRestartButton;

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
            _manager.InputListener.OnPause += PauseGame;
            _manager.OnGameDefeated += ShowDefeatedMenu;
            _resumeButton.onClick.AddListener(ResumeButtonClicked);
            _victoryQuitButton.onClick.AddListener(QuitButtonClicked);
            _defeatedQuitButton.onClick.AddListener(QuitButtonClicked);
            _victoryRestartButton.onClick.AddListener(RestartButtonClicked);
            _defeatedRestartButton.onClick.AddListener(RestartButtonClicked);
        }

        private void DestroyEvents()
        {
            _manager.InputListener.OnPause -= PauseGame;
            _manager.OnGameDefeated -= ShowDefeatedMenu;
            _resumeButton.onClick.RemoveListener(ResumeButtonClicked);
            _victoryQuitButton.onClick.RemoveListener(QuitButtonClicked);
            _defeatedQuitButton.onClick.RemoveListener(QuitButtonClicked);
            _victoryRestartButton.onClick.RemoveListener(RestartButtonClicked);
            _defeatedRestartButton.onClick.RemoveListener(RestartButtonClicked);
        }

        private void PauseGame()
        {
            if(_isPaused)
            {
                _isPaused = false;
                _pauseMenu.SetActive(false);
            }
            else
            {
                _isPaused = true;
                _pauseMenu.SetActive(true);
            }
        }

        private void ResumeButtonClicked()
        {
            OnPause?.Invoke();
            _isPaused = false;
            _pauseMenu.SetActive(false);
        }

        private void RestartButtonClicked()
        {
            _sceneLoader.RestartScene();
        }

        private void QuitButtonClicked()
        {
            _sceneLoader.LoadScene("MainMenu");
        }

        private void ShowDefeatedMenu()
        {
            _defeatedMenu.SetActive(true);
        }
    }
}
