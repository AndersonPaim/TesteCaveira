using System;
using Coimbra.Services.Events;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Event;
using Interfaces;
using Managers;
using Managers.Spawner;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private ScoreManager _scoreManager;
    [SerializeField] private EnemySpawnerController _enemySpawner;
    [SerializeField] private AudioManager _audioManager;
    [SerializeField] private GameObject _hudObject;
    [SerializeField] private GameObject _waveCounter;
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private TextMeshProUGUI _countdownTime;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _waveCounterText;
    [SerializeField] private SoundEffect _countdownSound;

    private IAudioPlayer _audioPlayer;

    public void StartCountdown(int time)
    {
        ShowStartCountdownASync(time);
    }

    private void Start()
    {
        SetupEvents();
        Initialize();
    }

    private void OnDestroy()
    {
        DestroyEvents();
    }

    private void Initialize()
    {
        _audioPlayer = _audioManager.GetComponent<IAudioPlayer>();
    }

    private void SetupEvents()
    {
        _playerController.OnInitializeHealth += InitializeHealthSlider;
        _playerController.OnUpdateHealth += UpdateHealthSlider;
        _scoreManager.OnUpdateScore += UpdateScore;
        _enemySpawner.OnStartWave += UpdateWaveCounter;
        OnGameStarted.AddListener(GameStarted);
        OnGameVictory.AddListener(GameVictory);
        OnGameDefeated.AddListener(GameDefeated);
    }


    private void DestroyEvents()
    {
        _playerController.OnInitializeHealth -= InitializeHealthSlider;
        _playerController.OnUpdateHealth -= UpdateHealthSlider;
        _scoreManager.OnUpdateScore -= UpdateScore;
        _enemySpawner.OnStartWave -= UpdateWaveCounter;
        OnGameStarted.RemoveAllListeners();
        OnGameVictory.RemoveAllListeners();
        OnGameDefeated.RemoveAllListeners();
    }

    private void InitializeHealthSlider(float health)
    {
        _healthSlider.maxValue = health;
        _healthSlider.value = health;
    }

    private void GameStarted(ref EventContext context, in OnGameStarted e)
    {
        EnableHUD();
    }

    private void GameVictory(ref EventContext context, in OnGameVictory e)
    {
        DisableHUD();
    }

    private void GameDefeated(ref EventContext context, in OnGameDefeated e)
    {
        DisableHUD();
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
        _scoreText.text = score.ToString();
    }

    private async void UpdateWaveCounter(int wave)
    {
        _waveCounterText.text = "WAVE " + wave.ToString();
        _waveCounter.transform.DOScale(1, 0.7f);
        await UniTask.Delay(2000);
        _waveCounter.transform.DOScale(0, 0.4f);
    }

    private async UniTask ShowStartCountdownASync(int time)
    {
        for(int i = time; i > 0; i--)
        {
            _countdownTime.text = i.ToString();
            _countdownTime.transform.DOScale(1, 0.7f);
            _countdownTime.transform.DOScale(0, 0.2f);
            _audioPlayer.PlayAudio(_countdownSound, transform.position);
            await UniTask.Delay(1000);
        }

        _countdownTime.gameObject.SetActive(false);
    }
}
