using UnityEngine;
using Cinemachine;
using System;
using Managers;
using Interfaces;

public class PlayerController : MonoBehaviour, IDamageable, IHealable
{
    public Action OnPlayerDie;

    public delegate void HealthHandler(float health);
    public HealthHandler OnUpdateHealth;
    public HealthHandler OnInitializeHealth;

    public Action OnTakeDamage;

    [SerializeField] private GameManager _manager;
    [SerializeField] private PlayerCamController _cameraController;
    [SerializeField] private PlayerBalancer _playerBalancer;
    [SerializeField] private GameObject _playerPivot;

    private float _speed = 0;
    private float _health;
    private float _maxHealth;
    private bool _isGrounded = true;
    private Rigidbody _rb;

    public void TakeDamage(float damage)
    {
        _health -= damage;
        OnTakeDamage?.Invoke();
        OnUpdateHealth?.Invoke(_health);

        if(_health <= 0)
        {
            OnPlayerDie?.Invoke();
        }
    }

    public void Heal(float healValue)
    {
        if(_health + healValue >= _maxHealth)
        {
            _health = _maxHealth;
        }
        else
        {
            _health += healValue;
        }

        OnUpdateHealth?.Invoke(_health);
    }

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
        _health = _playerBalancer.health;
        _maxHealth = _health;
        OnInitializeHealth?.Invoke(_health);
        SetupDelegates();
    }

    private void ReceiveInputs(InputData inputData)
    {
        Movement(inputData.Movement);
        Jump(inputData.isJumping);
    }

    private void Movement(Vector2 movement)
    {
        SetVelocity(movement.y, movement.x);
        movement *= _speed;
        Vector3 dir = new Vector3(-Camera.main.transform.right.z, 0, Camera.main.transform.right.x);
        Vector3 movementDir = (dir * movement.y + Camera.main.transform.right * movement.x);
        movementDir.y = _rb.velocity.y;

        _rb.velocity = movementDir;
    }

    private void SetVelocity(float directionY, float directionX)
    {
        if (directionY > 0 || directionX > 0 || directionX < 0)
        {
            if (_speed <= _playerBalancer.speed)
            {
                _speed += Time.deltaTime * _playerBalancer.acceleration;

                if (_speed > _playerBalancer.speed)
                {
                    _speed = _playerBalancer.speed;
                }
            }
        }
        else if (directionY == 0 && directionX == 0)
        {
            if (_speed > 0)
            {
                _speed -= Time.deltaTime * _playerBalancer.deceleration;
            }
            else if (_speed <= 0)
            {
                _speed = 0;
            }
        }
        else if (directionY < 0)
        {
            if (_speed <= _playerBalancer.speed * 0.75f)
            {
                _speed += Time.deltaTime * _playerBalancer.acceleration;
            }
        }
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

        if(_playerPivot.transform.rotation.eulerAngles.x + lookY > 280 || _playerPivot.transform.rotation.eulerAngles.x + lookY < 60)
        {
            _playerPivot.transform.Rotate(Vector3.right * lookY);
        }

        Vector3 yRot = Vector3.right * -lookY;
    }

    private void GroundCheck()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.3f);
    }

    // POWER UP:

    // AUMENTAR DANO
    // MATAR TODOS INIMIGOS
    // AUMENTAR VIDA
    // SPAWN POR WAVES - MODO RANDOM
}