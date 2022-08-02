using _Project.Scripts.Events;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Managers;
using Coimbra.Services;
using Coimbra.Services.Events;
using UnityEngine;

namespace _Project.Scripts.UI
{
    public class GameMenus : MonoBehaviour
    {
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
            _sceneLoader = ServiceLocator.Get<ISceneLoader>();
        }

        private void StartEvents()
        {
            OnPauseGame.AddListener(PauseGame);
            OnGameVictory.AddListener(ShowVictoryScreen);
            OnGameDefeated.AddListener(ShowDefeatedScreen);
        }

        private void DestroyEvents()
        {
            OnPauseGame.RemoveAllListeners();
            OnGameVictory.RemoveAllListeners();
            OnGameDefeated.RemoveAllListeners();
        }

        private void PauseGame(ref EventContext context, in OnPauseGame e)
        {
            if(e.IsPaused)
            {
                _pauseMenu.SetActive(true);
            }
            else
            {
                _pauseMenu.SetActive(false);
                _settingsMenu.SetActive(false);
            }
        }

        private void ShowDefeatedScreen(ref EventContext context, in OnGameDefeated e)
        {
            _defeatedMenu.SetActive(true);
        }

        private void ShowVictoryScreen(ref EventContext context, in OnGameVictory e)
        {
            _victoryMenu.SetActive(true);
        }
    }
}
