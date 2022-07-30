using System.Threading;
using _Project.Scripts.Managers;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Weapons
{
    public class BowController : MonoBehaviour
    {
        public delegate void PlayerDataHandler(PlayerData playerData);
        public PlayerDataHandler OnPlayerDataUpdate;

        [SerializeField] private InputListener _inputListener;
        [SerializeField] private Transform _shootPosition;
        [SerializeField] private GameObject _arrowPrefab;
        [SerializeField] private MeshRenderer _previewArrowMesh;
        [SerializeField] private Material _previewArrowDefaultMat;
        [SerializeField] private Material _previewBuffedArrowMat;
        [SerializeField] private float _shootForce;

        private bool _isAiming = false;
        private bool _isShooting = false;
        private float _damageMultiplier = 1;
        private PlayerData _playerData;
        private CancellationTokenSource _cancellationTokenSource;

        public void SetDamageBuff(float multiplier, int time)
        {
            _damageMultiplier = multiplier;
            _previewArrowMesh.material = _previewBuffedArrowMat;

            if(_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
            }

            DamageBuffTimeASync(time);
        }

        private void Start()
        {
            Initialize();
        }

        private void Update()
        {
            CreatePlayerStruct();
            _playerData = new PlayerData();
        }

        private void OnDestroy()
        {
            RemoveDelegates();
        }

        private void SetupDelegates()
        {
            _inputListener.OnInput += ReceiveInputs;
        }

        private void RemoveDelegates()
        {
            _inputListener.OnInput -= ReceiveInputs;
        }

        private void Initialize()
        {
            SetupDelegates();
        }
    
        private void ReceiveInputs(InputData inputData)
        {
            IsAiming(inputData.IsAiming);
            IsShooting(inputData.IsShooting);
        }

        private void CreatePlayerStruct()
        {
            _playerData.Aim = _isAiming;
            _playerData.Shoot = _isShooting;

            if(_isShooting)
            {
                _isShooting = false;
            }

            OnPlayerDataUpdate?.Invoke(_playerData);
        }

        private void IsAiming(bool isAiming)
        {
            _isAiming = isAiming;
        }

        private void IsShooting(bool isShooting)
        {
            _isShooting = isShooting;
        }

        private async UniTask DamageBuffTimeASync(int time)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            await UniTask.Delay((time * 1000), cancellationToken: _cancellationTokenSource.Token);
            _damageMultiplier = 1;
            _previewArrowMesh.material = _previewArrowDefaultMat;
        }

        private void ArrowShoot()
        {
            GameObject obj = ObjectPooler.sInstance.SpawnFromPool(_arrowPrefab.GetInstanceID());
            WeaponBase arrow = obj.GetComponent<WeaponBase>();
            Rigidbody rb = obj.GetComponent<Rigidbody>();

            arrow.SetDamageMultiplier(_damageMultiplier);
            obj.transform.position = _shootPosition.transform.position;
            obj.transform.rotation = _shootPosition.transform.rotation;
            rb.velocity = (_shootPosition.transform.forward * _shootForce);
        }
    }
}