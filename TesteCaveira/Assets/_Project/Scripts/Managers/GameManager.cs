using UnityEngine;
using UI;
using System;
using Cysharp.Threading.Tasks;
using Managers.Spawner;
using Coimbra.Services.Events;
using Coimbra.Services;
using Event;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private ObjectPooler _objectPooler;
        [SerializeField] private InputListener _inputListener;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private BowController _bowController;
        [SerializeField] private EnemySpawnerController _enemySpawnerController;
        [SerializeField] private UIManager _uiController;
        [SerializeField] private AudioManager _audioManager;
        [SerializeField] private PauseScreen _pauseScreen;
        [SerializeField] private SettingsScreen _settingsScreen;
        [SerializeField] private ScoreManager _scoreManager;
        [SerializeField] private WaypointController _waypointController;

        public ObjectPooler ObjectPooler => _objectPooler;
        public InputListener InputListener => _inputListener;
        public PlayerController PlayerController => _playerController;
        public BowController BowController => _bowController;
        public EnemySpawnerController EnemySpawnerController => _enemySpawnerController;
        public AudioManager AudioManager => _audioManager;
        public SettingsScreen SettingsScreen => _settingsScreen;
        public ScoreManager ScoreManager => _scoreManager;
        public WaypointController WaypointController => _waypointController;

        private bool _isPaused = false;
        private bool _gameOver = false;
        private IEventService _eventService;

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
            _eventService = ServiceLocator.Get<IEventService>();
            OnGameStarted gameStarted = new OnGameStarted();
            gameStarted?.Invoke(_eventService);
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
            }
            else
            {
                _isPaused = true;
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0;
            }

            OnPauseGame pauseGame = new OnPauseGame() {IsPaused = _isPaused };
            pauseGame?.Invoke(_eventService);
        }

        private void Victory()
        {
            if(_gameOver)
            {
                return;
            }

            _gameOver = true;
            PauseGameFade();

            OnGameVictory gameVictory = new OnGameVictory();
            gameVictory?.Invoke(_eventService);
        }

        private void Defeated()
        {
            if(_gameOver)
            {
                return;
            }

            _gameOver = true;
            PauseGameFade();

            OnGameDefeated gameDefeated = new OnGameDefeated();
            gameDefeated?.Invoke(_eventService);
        }

        private async UniTask PauseGameFade()
        {
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0.3f;
            await UniTask.Delay(1000);
            Time.timeScale = 0;
        }
    }

    //TODO
    //ARQUEIRO REPOSICIONAR AO TOMAR DANO
    //MELEE DEFENDER APÓS CERTO TEMPO
}