using UnityEngine;
using UI;
using System;
using Cysharp.Threading.Tasks;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public Action OnGameStarted;
        public Action OnGameVictory;
        public Action OnGameDefeated;
        public Action<bool> OnPauseGame;

        [SerializeField] private ObjectPooler _objectPooler;
        [SerializeField] private InputListener _inputListener;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private BowController _bowController;
        [SerializeField] private EnemySpawnerController _enemySpawnerController;
        [SerializeField] private SceneController _sceneController;
        [SerializeField] private UIManager _uiController;
        [SerializeField] private AudioManager _audioManager;
        [SerializeField] private PauseScreen _pauseScreen;
        [SerializeField] private SettingsScreen _settingsScreen;
        [SerializeField] private ScoreManager _scoreManager;

        public ObjectPooler ObjectPooler => _objectPooler;
        public InputListener InputListener => _inputListener;
        public PlayerController PlayerController => _playerController;
        public BowController BowController => _bowController;
        public EnemySpawnerController EnemySpawnerController => _enemySpawnerController;
        public SceneController SceneController => _sceneController;
        public AudioManager AudioManager => _audioManager;
        public SettingsScreen SettingsScreen => _settingsScreen;
        public ScoreManager ScoreManager => _scoreManager;

        private bool _isPaused = false;
        private bool _gameOver = false;

        private async UniTask Start()
        {
            StartEvents();
            await Initialize();
        }

        private void OnDestroy()
        {
            DestroyEvents();
        }

        private void StartEvents()
        {
            _inputListener.OnPause += PauseGame;
            _pauseScreen.OnResumeGame += PauseGame;
            _playerController.OnPlayerDie += Defeated;
            _enemySpawnerController.OnFinishWaves += Victory;
        }

        private void DestroyEvents()
        {
            _inputListener.OnPause -= PauseGame;
            _pauseScreen.OnResumeGame -= PauseGame;
            _playerController.OnPlayerDie -= Defeated;
            _enemySpawnerController.OnFinishWaves -= Victory;
        }

        private async UniTask Initialize()
        {
            Time.timeScale = 1;
            SaveData data = SaveSystem.Load();
            _uiController.StartCountdown(data.StartCountdown);
            await UniTask.Delay(data.StartCountdown * 1000);
            OnGameStarted?.Invoke();
        }

        private void PauseGame()
        {
            if(_gameOver)
            {
                return;
            }

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
            _gameOver = true;
            PauseGameFade();
            OnGameDefeated?.Invoke();
        }

        private async UniTask PauseGameFade()
        {
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0.3f;
            await UniTask.Delay(1000);
            Time.timeScale = 0;
        }
    }
}