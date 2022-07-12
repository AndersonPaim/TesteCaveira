using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ObjectPooler _objectPooler;
    [SerializeField] private InputListener _inputListener;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private BowController _bowController;

    public ObjectPooler ObjectPooler => _objectPooler;
    public InputListener InputListener => _inputListener;
    public PlayerController PlayerController => _playerController;
    public BowController BowController => _bowController;
}