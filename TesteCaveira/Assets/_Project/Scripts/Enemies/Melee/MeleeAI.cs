using UnityEngine.AI;
using UnityEngine;
using Interfaces;
using System;

namespace Enemy.Melee
{
    public class MeleeAI : MonoBehaviour, IDamageable
    {
        public static Action OnEnemyDie;

        [SerializeField] private EnemyBalancer _enemyBalancer;

        private NavMeshAgent _agent;
        private Animator _anim;
        private StateMachine _currentState;
        private NavMeshPath _path;
        private Events _state;
        private GameObject _player;
        private float _health;
        private bool _isDead = false;

        public bool CanAttackPlayer()
        {
            Vector3 direction = _player.transform.position - gameObject.transform.position;
            float viewAngle = Vector3.Angle(direction, gameObject.transform.forward);

            if(direction.magnitude < _enemyBalancer.attackDistance && viewAngle < _enemyBalancer.viewAngle)
            {
                return true;
            }

            return false;
        }

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
                OnEnemyDie?.Invoke();
                _currentState.MeleeDeath();
                _isDead = true;
            }
        }

        public void SetupEnemy(GameManager manager)
        {
            _player = manager.PlayerController.gameObject;
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
