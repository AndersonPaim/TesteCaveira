using _Project.Scripts.Interfaces;
using _Project.Scripts.Managers;
using Coimbra.Services;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class GameOverScreen : MonoBehaviour
    {
        [SerializeField] private ScoreManager _scoreManager;
        [SerializeField] private SceneController _sceneController;
        [SerializeField] private GameObject _loadingScreen;
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
            _sceneLoader = ServiceLocator.Get<ISceneLoader>();
            _scoreText.text = _scoreManager.GetScore().ToString();
            _killsText.text = _scoreManager.GetKills().ToString();
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
            _loadingScreen.SetActive(true);
            _sceneLoader.RestartScene();
        }

        private void QuitButtonClicked()
        {
            _loadingScreen.SetActive(true);
            _sceneLoader.LoadScene("MainMenu");
        }
    }
}
