using UnityEngine;
using UnityEngine.AI;

namespace Enemy.Archer
{
    public class ArcherAI : MonoBehaviour
    {
        [SerializeField] private EnemyBalancer _enemyBalancer;
        [SerializeField] private Transform _shootPosition;
        [SerializeField] private GameManager _manager;
        [SerializeField] private GameObject _player;

        private NavMeshAgent _agent;
        private Animator _anim;
        private StateMachine _currentState;
        private NavMeshPath _path;

        public void Attack()
        {
            GameObject arrow = _manager.ObjectPooler.SpawnFromPool(ObjectsTag.Arrow);
            arrow.transform.position =  _shootPosition.position;
            Rigidbody rb = arrow.GetComponent<Rigidbody>();
            rb.velocity = _shootPosition.transform.forward * _enemyBalancer.shootForce;
        }

        private void Start()
        {
            _anim = GetComponent<Animator>();
            _agent = GetComponent<NavMeshAgent>();
            _path = new NavMeshPath();
            _currentState = new ArcherSpawn(gameObject, _player, _agent, _anim, _path, _enemyBalancer);
        }

        private void Update()
        {
            _currentState = _currentState.Process();
        }
    }
}

