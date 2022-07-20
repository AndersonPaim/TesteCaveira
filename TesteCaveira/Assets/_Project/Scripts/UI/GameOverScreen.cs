using Interfaces;
using Managers;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace UI
{
    public class GameOverScreen : MonoBehaviour
    {
        [SerializeField] private GameManager _manager;
        [SerializeField] private Transform _popUpScreen;
        [SerializeField] private Button _quitButton;
        [SerializeField] private Button _restartButton;

        private ISceneLoader _sceneLoader;

        private void OnEnable()
        {
            EntryAnimation();
        }

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
            _quitButton.onClick.AddListener(QuitButtonClicked);
            _restartButton.onClick.AddListener(RestartButtonClicked);
        }

        private void DestroyEvents()
        {
            _quitButton.onClick.RemoveListener(QuitButtonClicked);
            _restartButton.onClick.RemoveListener(RestartButtonClicked);
        }

        private void EntryAnimation()
        {
            _popUpScreen.transform.DOScale(0.8f, 0.3f);
        }

        private void RestartButtonClicked()
        {
            _sceneLoader.RestartScene();
        }

        private void QuitButtonClicked()
        {
            _sceneLoader.LoadScene("MainMenu");
        }
    }
}
