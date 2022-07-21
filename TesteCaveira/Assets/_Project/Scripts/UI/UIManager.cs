using Cysharp.Threading.Tasks;
using DG.Tweening;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameManager _manager;
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private TextMeshProUGUI _countdownTime;

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
    }

    private void DestroyEvents()
    {
        _manager.PlayerController.OnInitializeHealth -= InitializeHealthSlider;
        _manager.PlayerController.OnUpdateHealth -= UpdateHealthSlider;
    }

    private void InitializeHealthSlider(float health)
    {
        _healthSlider.maxValue = health;
        _healthSlider.value = health;
    }

    private void UpdateHealthSlider(float health)
    {
        _healthSlider.value = health;
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
