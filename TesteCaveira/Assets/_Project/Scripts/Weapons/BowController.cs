using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowController : MonoBehaviour
{
    public delegate void PlayerDataHandler(PlayerData playerData);
    public PlayerDataHandler OnPlayerDataUpdate;

    [SerializeField] private Transform _shootPosition;
    [SerializeField] private float _shootForce;
    [SerializeField] private GameManager _gameManager;

    private bool _isPaused = false;
    private bool _isAiming = false;
    private bool _isShooting = false;
    private bool _isChangingArrow = false;
    private ObjectPooler _objectPooler;
    private PlayerData _playerData;

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

    private void ArrowShoot()
    {
        GameObject obj = _objectPooler.SpawnFromPool(ObjectsTag.Arrow);

        obj.transform.position = _shootPosition.transform.position;
        obj.transform.rotation = _shootPosition.transform.rotation;

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        rb.velocity = (_shootPosition.transform.forward * _shootForce);
    }
}