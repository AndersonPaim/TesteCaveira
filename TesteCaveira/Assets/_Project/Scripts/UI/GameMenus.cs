using Interfaces;
using Managers;
using UnityEngine;

namespace UI
{
    public class GameMenus : MonoBehaviour
    {
        [SerializeField] private GameManager _manager;
        [SerializeField] private GameObject _pauseMenu;
        [SerializeField] private GameObject _victoryMenu;
        [SerializeField] private GameObject _defeatedMenu;

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
        }

        private void DestroyEvents()
        {
            _manager.InputListener.OnPause -= PauseGame;
            _manager.OnGameDefeated -= ShowDefeatedScreen;
            _manager.OnGameVictory -= ShowVictoryScreen;
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
