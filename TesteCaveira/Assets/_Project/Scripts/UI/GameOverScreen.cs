using Interfaces;
using Managers;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

namespace UI
{
    public class GameOverScreen : MonoBehaviour
    {
        [SerializeField] private GameManager _manager;
        [SerializeField] private Transform _popUpScreen;
        [SerializeField] private Button _quitButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _killsText;

        private ISceneLoader _sceneLoader;

        private void OnEnable()
        {
            EntryAnimation();
            Initialize();
        }

        private void Start()
        {
            StartEvents();
        }

        private void OnDisable()
        {
            DestroyEvents();
        }

        private void Initialize()
        {
            _sceneLoader = _manager.SceneController.GetComponent<ISceneLoader>();
            _scoreText.text = _manager.ScoreManager.GetScore().ToString();
            _killsText.text = _manager.ScoreManager.GetKills().ToString();
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
