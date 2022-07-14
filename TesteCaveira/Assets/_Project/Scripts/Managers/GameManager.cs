using UnityEngine;
using UI;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private ObjectPooler _objectPooler;
        [SerializeField] private InputListener _inputListener;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private BowController _bowController;
        [SerializeField] private GameMenusController _gameMenusController;

        public ObjectPooler ObjectPooler => _objectPooler;
        public InputListener InputListener => _inputListener;
        public PlayerController PlayerController => _playerController;
        public BowController BowController => _bowController;
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
        }

        private void DestroyEvents()
        {
            _inputListener.OnPause -= PauseGame;
            _gameMenusController.OnPause -= PauseGame;
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
    }
}