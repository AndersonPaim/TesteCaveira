using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private SceneController _sceneController;
        [SerializeField] private Slider _loadingBar;
        [SerializeField] private TextMeshProUGUI _loadingProgressText;

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
            _sceneController.OnUpdateProgress += LoadingScreenProgress;
        }

        private void DestroyEvents()
        {
            _sceneController.OnUpdateProgress -= LoadingScreenProgress;
        }

        private void LoadingScreenProgress(float progress)
        {
            _loadingBar.value = progress;
            _loadingProgressText.text = "Loading " + (progress * 100).ToString() + "%";
        }
    }
}