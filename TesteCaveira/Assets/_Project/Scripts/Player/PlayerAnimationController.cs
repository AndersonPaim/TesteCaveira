using Managers;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    private Animator _animator;

    private void Start()
    {
        Initialize();
        SetupDelegates();
    }

    private void OnDestroy()
    {
        RemoveDelegates();
    }

    private void SetupDelegates()
    {
        _gameManager.BowController.OnPlayerDataUpdate += ReceiveBowData;
        _gameManager.PlayerController.OnTakeDamage += TakeDamage;
    }

    private void RemoveDelegates()
    {
        _gameManager.BowController.OnPlayerDataUpdate -= ReceiveBowData;
        _gameManager.PlayerController.OnTakeDamage -= TakeDamage;
    }

    private void Initialize()
    {
        _animator = GetComponent<Animator>();
    }

    private void ReceiveBowData(PlayerData playerData)
    {
        Aim(playerData.Aim);
        Shoot(playerData.Shoot);
    }

    private void Aim(bool isAiming)
    {
        _animator.SetBool("isAiming", isAiming);
    }

    private void Shoot(bool isShooting)
    {
        _animator.SetBool("isShooting", isShooting);
    }

    private void TakeDamage()
    {
        _animator.SetTrigger("takeDamage");
    }
}