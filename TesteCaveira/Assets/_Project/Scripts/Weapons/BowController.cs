using System.Threading;
using Cysharp.Threading.Tasks;
using Managers;
using UnityEngine;
using Weapons;

public class BowController : MonoBehaviour
{
    public delegate void PlayerDataHandler(PlayerData playerData);
    public PlayerDataHandler OnPlayerDataUpdate;

    [SerializeField] private Transform _shootPosition;
    [SerializeField] private float _shootForce;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private MeshRenderer _previewArrowMesh;
    [SerializeField] private Material _previewArrowDefaultMat;
    [SerializeField] private Material _previewBuffedArrowMat;

    private bool _isPaused = false;
    private bool _isAiming = false;
    private bool _isShooting = false;
    private bool _isChangingArrow = false;
    private float _damageMultiplier = 1;
    private ObjectPooler _objectPooler;
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
        _gameManager.InputListener.OnInput += ReceiveInputs;
    }

    private void RemoveDelegates()
    {
        _gameManager.InputListener.OnInput -= ReceiveInputs;
    }

    private void Initialize()
    {
        SetupDelegates();
        _objectPooler = _gameManager.ObjectPooler;
    }

    private void PauseInputs(bool isPaused)
    {
        _isPaused = isPaused;
    }

    private void ReceiveInputs(InputData inputData)
    {
        IsAiming(inputData.isAiming);
        IsShooting(inputData.isShooting);
    }

    private void CreatePlayerStruct()
    {
        _playerData.Aim = _isAiming;
        _playerData.Shoot = _isShooting;

        if(_isShooting)
        {
            _isShooting = false;
        }
        if(_isChangingArrow)
        {
            _isChangingArrow = false;
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
        GameObject obj = _objectPooler.SpawnFromPool(ObjectsTag.Arrow);
        WeaponBase arrow = obj.GetComponent<WeaponBase>();
        Rigidbody rb = obj.GetComponent<Rigidbody>();

        arrow.SetDamageMultiplier(_damageMultiplier);
        obj.transform.position = _shootPosition.transform.position;
        obj.transform.rotation = _shootPosition.transform.rotation;
        rb.velocity = (_shootPosition.transform.forward * _shootForce);
    }
}