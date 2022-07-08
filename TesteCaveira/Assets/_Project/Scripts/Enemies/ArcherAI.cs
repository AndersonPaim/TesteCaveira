using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ArcherAI : MonoBehaviour
{
    [SerializeField] private GameObject _player;

    private NavMeshAgent _agent;
    private Animator _animator;
    private StateMachine _currentState;

    private void Start()
    {
        _currentState = new Idle();
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        _currentState = _currentState.Process();
    }
}

