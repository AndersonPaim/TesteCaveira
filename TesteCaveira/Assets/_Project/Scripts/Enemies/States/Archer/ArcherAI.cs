using UnityEngine;
using UnityEngine.AI;
using Interfaces;
using Managers;

namespace Enemy.Archer
{
    public class ArcherAI : MonoBehaviour, IDamageable
    {
        public delegate void EnemyHandler(GameObject enemy, int score);
        public EnemyHandler OnEnemyDie;

        [SerializeField] private EnemyBalancer _enemyBalancer;
        [SerializeField] private SkinnedMeshRenderer _mesh;
        [SerializeField] private Transform _shootPosition;
        [SerializeField] private EnemyAudioController _audioController;
        [SerializeField] private GameObject _arrowPrefab;

        private GameManager _manager;
        private NavMeshAgent _agent;
        private Animator _anim;
        private StateMachine _currentState;
        private NavMeshPath _path;
        private GameObject _player;
        private float _health;
        private bool _isDead = false;

        public void Attack()
        {
            GameObject arrow = _manager.ObjectPooler.SpawnFromPool(_arrowPrefab.GetInstanceID());
            arrow.transform.position =  _shootPosition.position;
            Rigidbody rb = arrow.GetComponent<Rigidbody>();
            rb.velocity = _shootPosition.transform.forward * _enemyBalancer.shootForce;
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
                _currentState.TakeDamage(Enemies.ARCHER);
            }
            else
            {
                _isDead = true;
                OnEnemyDie?.Invoke(gameObject, _enemyBalancer.killScore);
                _currentState.Death();
            }
        }
        public void SetupEnemy(GameManager manager)
        {
            _player = manager.PlayerController.gameObject;
            _manager = manager;
            _audioController.SetupManager(_manager.AudioManager);
            _currentState = new EnemySpawn(gameObject, _player, _agent, _mesh, _anim, _path, _enemyBalancer, manager, Enemies.ARCHER);
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
            _currentState = _currentState.Process();
        }
    }
}

