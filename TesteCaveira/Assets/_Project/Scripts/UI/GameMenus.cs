using Coimbra.Services;
using Interfaces;
using Managers;
using UnityEngine;

namespace UI
{
    public class GameMenus : MonoBehaviour
    {
        [SerializeField] private GameManager _manager;
        [SerializeField] private SceneController _sceneController;
        [SerializeField] private GameObject _pauseMenu;
        [SerializeField] private GameObject _victoryMenu;
        [SerializeField] private GameObject _defeatedMenu;
        [SerializeField] private GameObject _settingsMenu;

        private ISceneLoader _sceneLoader;

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
            _sceneLoader = _sceneController.GetComponent<ISceneLoader>(); //ServiceLocator.Get<ISceneLoader>();
        }

        private void StartEvents()
        {
            _manager.OnPauseGame += PauseGame;
            _manager.OnGameDefeated += ShowDefeatedScreen;
            _manager.OnGameVictory += ShowVictoryScreen;
        }

        private void DestroyEvents()
        {
            _manager.OnPauseGame -= PauseGame;
            _manager.OnGameDefeated -= ShowDefeatedScreen;
            _manager.OnGameVictory -= ShowVictoryScreen;
        }

        private void PauseGame(bool isPaused)
        {
            if(isPaused)
            {
                _pauseMenu.SetActive(true);
            }
            else
            {
                _pauseMenu.SetActive(false);
                _settingsMenu.SetActive(false);
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
