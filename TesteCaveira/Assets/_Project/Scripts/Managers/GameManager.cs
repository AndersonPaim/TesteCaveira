using UnityEngine;
using UI;
using System;
using Cysharp.Threading.Tasks;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public Action OnGameVictory;
        public Action OnGameDefeated;
        public Action<bool> OnPauseGame;

        [SerializeField] private ObjectPooler _objectPooler;
        [SerializeField] private InputListener _inputListener;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private BowController _bowController;
        [SerializeField] private EnemySpawnerController _enemySpawnerController;
        [SerializeField] private SceneController _sceneController;
        [SerializeField] private PauseScreen _pauseScreen;

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
            Time.timeScale = 1;
        }

        private void OnDestroy()
        {
            DestroyEvents();
        }

        private void StartEvents()
        {
            _inputListener.OnPause += PauseGame;
            _pauseScreen.OnPause += PauseGame;
            _playerController.OnPlayerDie += Defeated;
            _enemySpawnerController.OnFinishWaves += Victory;
        }

        private void DestroyEvents()
        {
            _inputListener.OnPause -= PauseGame;
            _pauseScreen.OnPause -= PauseGame;
            _playerController.OnPlayerDie -= Defeated;
            _enemySpawnerController.OnFinishWaves -= Victory;
        }

        private void PauseGame()
        {
            if(_isPaused)
            {
                _isPaused = false;
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1;
                OnPauseGame?.Invoke(_isPaused);
            }
            else
            {
                _isPaused = true;
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0;
                OnPauseGame?.Invoke(_isPaused);
            }
        }

        private void Victory()
        {
            PauseGameFade();
            OnGameVictory?.Invoke();
        }

        private void Defeated()
        {
            PauseGameFade();
            OnGameDefeated?.Invoke();
        }

        private async UniTask PauseGameFade()
        {
            Time.timeScale = 0.3f;
            await UniTask.Delay(800);
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}