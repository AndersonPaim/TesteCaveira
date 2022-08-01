using _Project.Scripts.Events;
using _Project.Scripts.Player;
using _Project.Scripts.UI;
using Coimbra.Services;
using Coimbra.Services.Events;
using Cysharp.Threading.Tasks;
using UI;
using UnityEngine;

namespace _Project.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private InputListener _inputListener;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private EnemySpawnerController _enemySpawnerController;
        [SerializeField] private UIManager _uiController;
        [SerializeField] private PauseScreen _pauseScreen;

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
}