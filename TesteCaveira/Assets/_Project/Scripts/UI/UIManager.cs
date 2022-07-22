using Cysharp.Threading.Tasks;
using DG.Tweening;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameManager _manager;
    [SerializeField] private GameObject _hudObject;
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private TextMeshProUGUI _countdownTime;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _waveCounterText;

    public void StartCountdown(int time)
    {
        ShowStartCountdownASync(time);
    }

    private void Start()
    {
        SetupEvents();
    }

    private void OnDestroy()
    {
        DestroyEvents();
    }

    private void SetupEvents()
    {
        _manager.PlayerController.OnInitializeHealth += InitializeHealthSlider;
        _manager.PlayerController.OnUpdateHealth += UpdateHealthSlider;
        _manager.ScoreManager.OnUpdateScore += UpdateScore;
        _manager.EnemySpawnerController.OnStartWave += UpdateWaveCounter;
        _manager.OnGameStarted += EnableHUD;
        _manager.OnGameVictory += DisableHUD;
        _manager.OnGameDefeated += DisableHUD;
    }

    private void DestroyEvents()
    {
        _manager.PlayerController.OnInitializeHealth -= InitializeHealthSlider;
        _manager.PlayerController.OnUpdateHealth -= UpdateHealthSlider;
        _manager.ScoreManager.OnUpdateScore -= UpdateScore;
        _manager.EnemySpawnerController.OnStartWave -= UpdateWaveCounter;
        _manager.OnGameStarted -= EnableHUD;
        _manager.OnGameVictory -= DisableHUD;
        _manager.OnGameDefeated -= DisableHUD;
    }

    private void InitializeHealthSlider(float health)
    {
        _healthSlider.maxValue = health;
        _healthSlider.value = health;
    }

    private void EnableHUD()
    {
        _hudObject.SetActive(true);
    }

    private void DisableHUD()
    {
        _hudObject.SetActive(true);
    }

    private void UpdateHealthSlider(float health)
    {
        _healthSlider.value = health;
    }

    private void UpdateScore(int score)
    {
        _scoreText.text = "SCORE: " + score.ToString();
    }

    private void UpdateWaveCounter(int wave)
    {
        _waveCounterText.text = "WAVE: " + wave.ToString();
    }

    private async UniTask ShowStartCountdownASync(int time)
    {
        for(int i = time; i > 0; i--)
        {
            _countdownTime.text = i.ToString();
            _countdownTime.transform.DOScale(1, 0.7f);
            _countdownTime.transform.DOScale(0, 0.2f);
            await UniTask.Delay(1000);
        }

        _countdownTime.gameObject.SetActive(false);
    }
}
