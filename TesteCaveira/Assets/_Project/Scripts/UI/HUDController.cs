using Managers;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [SerializeField] private GameManager _manager;
    [SerializeField] private Slider _healthSlider;

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
}
