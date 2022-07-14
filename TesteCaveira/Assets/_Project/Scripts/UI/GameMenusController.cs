using System;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameMenusController : MonoBehaviour
    {
        public Action OnPause;

        [SerializeField] private GameManager _manager;
        [SerializeField] private GameObject _pauseMenu;
        [SerializeField] private Button _resumeButton;
        private bool _isPaused = false;

        private void Start()
        {
            StartEvents();
        }

        private void OnDisable()
        {
            DestroyEvents();
        }

        private void StartEvents()
        {
            _manager.InputListener.OnPause += PauseGame;
            _resumeButton.onClick.AddListener(ResumeButtonClicked);
        }

        private void DestroyEvents()
        {
            _manager.InputListener.OnPause -= PauseGame;
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
    }
}
