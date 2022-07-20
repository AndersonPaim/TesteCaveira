using Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private SceneController _sceneController;
        [SerializeField] private GameObject _settingsMenu;
        [SerializeField] private GameObject _loadingMenu;
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _quitButton;
        [SerializeField] private Button _settingsButton;

        private ISceneLoader _sceneLoader;

        private void Start()
        {
            Initialize();
            StartEvents();
        }

        private void OnDestroy()
        {
            DestroyEvents();
        }

        private void Initialize()
        {
            _sceneLoader = _sceneController.GetComponent<ISceneLoader>();
        }

        private void StartEvents()
        {
            _playButton.onClick.AddListener(PlayButtonClicked);
            _quitButton.onClick.AddListener(QuitButtonClicked);
            _settingsButton.onClick.AddListener(SettingsButtonClicked);
        }

        private void DestroyEvents()
        {
            _playButton.onClick.RemoveListener(PlayButtonClicked);
            _quitButton.onClick.RemoveListener(QuitButtonClicked);
            _settingsButton.onClick.RemoveListener(SettingsButtonClicked);
        }

        private void PlayButtonClicked()
        {
            _loadingMenu.SetActive(true);
            _sceneLoader.LoadScene("Game");
        }

        private void QuitButtonClicked()
        {
            Application.Quit();
        }

        private void SettingsButtonClicked()
        {
            _settingsMenu.SetActive(true);
        }
    }
}