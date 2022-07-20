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
            _manager.OnGameDefeated += ShowDefeatedScreen;
            _manager.OnGameVictory += ShowVictoryScreen;
            _resumeButton.onClick.AddListener(ResumeButtonClicked);
        }

        private void DestroyEvents()
        {
            _manager.InputListener.OnPause -= PauseGame;
            _manager.OnGameDefeated -= ShowDefeatedScreen;
            _manager.OnGameVictory -= ShowVictoryScreen;
            _resumeButton.onClick.RemoveListener(ResumeButtonClicked);
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

        private void ShowDefeatedScreen()
        {
            _defeatedMenu.SetActive(true);
        }

        private void ShowVictoryScreen()
        {
            _victoryMenu.SetActive(true);
        }
    }
}
