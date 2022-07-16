using UnityEngine.AI;
using UnityEngine;
using Interfaces;
using Managers;
using System;

namespace Enemy.Melee
{
    public class MeleeAI : MonoBehaviour, IDamageable
    {
        public delegate void EnemyHandler(GameObject enemy);
        public EnemyHandler OnEnemyDie;

        [SerializeField] private EnemyBalancer _enemyBalancer;

        private NavMeshAgent _agent;
        private Animator _anim;
        private StateMachine _currentState;
        private NavMeshPath _path;
        private GameObject _player;
        private float _health;
        private bool _isDead = false;

        public void TakeDamage(float damage)
        {
            if(_isDead)
            {
                return;
            }

            _health -= damage;

            if(_health > 0)
            {
                _currentState.MeleeDamage();
            }
            else
            {
                OnEnemyDie?.Invoke(gameObject);
                _currentState.MeleeDeath();
                _isDead = true;
            }
        }

        public void SetupEnemy(GameManager manager)
        {
            _player = manager.PlayerController.gameObject;
            _agent.enabled = true;
            _currentState = new MeleeSpawn(gameObject, _player, _agent, _anim, _path, _enemyBalancer);
            _health = _enemyBalancer.health;
            _isDead = false;
        }

        private void Awake()
        {
            _anim = GetComponent<Animator>();
            _agent = GetComponent<NavMeshAgent>();
            _path = new NavMeshPath();
        }

        private void Update()
        {
            if(_currentState != null)
            {
                _currentState = _currentState.Process();
            }
        }
    }
}
