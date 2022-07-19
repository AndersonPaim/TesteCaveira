using UnityEngine;
using UI;
using System;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public Action OnGameVictory;
        public Action OnGameDefeated;

        [SerializeField] private ObjectPooler _objectPooler;
        [SerializeField] private InputListener _inputListener;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private BowController _bowController;
        [SerializeField] private GameMenusController _gameMenusController;
        [SerializeField] private EnemySpawnerController _enemySpawnerController;
        [SerializeField] private SceneController _sceneController;

        public ObjectPooler ObjectPooler => _objectPooler;
        public InputListener InputListener => _inputListener;
        public PlayerController PlayerController => _playerController;
        public BowController BowController => _bowController;
        public EnemySpawnerController EnemySpawnerController => _enemySpawnerController;
        public SceneController SceneController => _sceneController;

        private bool _isPaused = false;

        private void Start()
        {
            StartEvents();
        }

        private void OnDestroy()
        {
            DestroyEvents();
        }

        private void StartEvents()
        {
            _inputListener.OnPause += PauseGame;
            _gameMenusController.OnPause += PauseGame;
            _playerController.OnPlayerDie += Defeated;
        }

        private void DestroyEvents()
        {
            _inputListener.OnPause -= PauseGame;
            _gameMenusController.OnPause -= PauseGame;
            _playerController.OnPlayerDie -= Defeated;
        }

        private void PauseGame()
        {
            if(_isPaused)
            {
                _isPaused = false;
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1;
            }
            else
            {
                _isPaused = true;
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0;
            }
        }

        private void Victory()
        {

        }

        private void Defeated()
        {
            OnGameDefeated?.Invoke();
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
        }

    }
}