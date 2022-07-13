using UnityEngine;
using UnityEngine.AI;
using Interfaces;

namespace Enemy.Archer
{
    public class ArcherAI : MonoBehaviour, IDamageable
    {
        [SerializeField] private EnemyBalancer _enemyBalancer;
        [SerializeField] private Transform _shootPosition;
        [SerializeField] private GameManager _manager;
        [SerializeField] private GameObject _player;

        private NavMeshAgent _agent;
        private Animator _anim;
        private StateMachine _currentState;
        private NavMeshPath _path;
        private float _health;

        public void Attack()
        {
            GameObject arrow = _manager.ObjectPooler.SpawnFromPool(ObjectsTag.Arrow);
            arrow.transform.position =  _shootPosition.position;
            Rigidbody rb = arrow.GetComponent<Rigidbody>();
            rb.velocity = _shootPosition.transform.forward * _enemyBalancer.shootForce;
        }

        public void TakeDamage(float damage)
        {
            _health -= damage;

            if(_health > 0)
            {
                _currentState.ArcherDamage();
            }
            else
            {
                _currentState.ArcherDeath();
            }
        }

        private void Start()
        {
            _anim = GetComponent<Animator>();
            _agent = GetComponent<NavMeshAgent>();
            _path = new NavMeshPath();
            _health = _enemyBalancer.health;
            _currentState = new ArcherSpawn(gameObject, _player, _agent, _anim, _path, _enemyBalancer);
        }

        private void Update()
        {
            _currentState = _currentState.Process();
        }
    }
}

