using _Project.Scripts.Weapons;
using UnityEngine;

namespace _Project.Scripts.Player
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private BowController _bowController;
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
            _bowController.OnPlayerDataUpdate += ReceiveBowData;
            _playerController.OnTakeDamage += TakeDamage;
            _playerController.OnPlayerDie += Death;
        }

        private void RemoveDelegates()
        {
            _bowController.OnPlayerDataUpdate -= ReceiveBowData;
            _playerController.OnTakeDamage -= TakeDamage;
            _playerController.OnPlayerDie -= Death;
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

        private void Death()
        {
            _animator.SetTrigger("death");
        }
    }
}