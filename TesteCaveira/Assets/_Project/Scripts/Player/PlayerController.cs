using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Audio;


public class PlayerController : MonoBehaviour
{
    public delegate void PlayerDataHandler(PlayerData playerData);
    public PlayerDataHandler OnPlayerDataUpdate;

    [SerializeField] private GameManager _manager;
    [SerializeField] private PlayerCamController _cameraController;
    [SerializeField] private PlayerBalancer _playerBalancer;
    [SerializeField] private GameObject _playerPivot;
    [SerializeField] private CinemachineVirtualCamera _virtualCam;

    private float _speed;
    private float _health;
    private float _maxHealth;

    private Vector2 _movement;

    private bool _isAiming = false;
    private bool _isGrounded = true;
    private bool _isJumping = false;
    private bool _isTakingDamage = false;
    private bool _isPaused = false;

    private Rigidbody _rb;
    private PlayerData _playerData;
    private ObjectPooler _objectPooler;

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        GroundCheck();
    }

    private void OnDestroy()
    {
        RemoveDelegates();
    }

    private void SetupDelegates()
    {
        _manager.InputListener.OnInput += ReceiveInputs;
        _cameraController.OnCameraRotate += Rotate;
    }

    private void RemoveDelegates()
    {
        _manager.InputListener.OnInput -= ReceiveInputs;
        _cameraController.OnCameraRotate -= Rotate;
    }

    private void Initialize()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _rb = GetComponent<Rigidbody>();
        _objectPooler = _manager.ObjectPooler;
        _playerData = new PlayerData();
        SetupDelegates();
    }

    private void PauseInputs(bool isPaused)
    {
        _isPaused = isPaused;
    }

    private void ReceiveInputs(InputData inputData)
    {
        if (!_isPaused)
        {
            Movement(inputData.Movement);
            Jump(inputData.isJumping);
            //Aim(inputData.isAiming);
        }
    }

    private void Movement(Vector2 movement)
    {
        movement *= _playerBalancer.speed;
        Vector3 dir = new Vector3(-Camera.main.transform.right.z, 0, Camera.main.transform.right.x);
        Vector3 movementDir = (dir * movement.y + Camera.main.transform.right * movement.x + Vector3.up * _rb.velocity.y);

        _rb.velocity = movementDir;
    }

    private void Jump(bool isJumping)
    {
        if (isJumping)
        {
            if (_isGrounded)
            {
                _rb.AddForce(Vector3.up * _playerBalancer.jumpForce, ForceMode.Impulse);
            }
        }
    }

    private void Rotate(float lookX, float lookY)
    {
        gameObject.transform.Rotate(Vector3.up * lookX);

        if(_playerPivot.transform.rotation.eulerAngles.x - lookY > 280 || _playerPivot.transform.rotation.eulerAngles.x - lookY < 60)
        {
            _playerPivot.transform.Rotate(Vector3.right * lookY);
        }

        Vector3 yRot = Vector3.right * -lookY;
    }

    private void GroundCheck()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.3f);
    }
}