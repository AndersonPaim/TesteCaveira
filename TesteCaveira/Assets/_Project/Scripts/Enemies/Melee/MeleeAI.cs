using UnityEngine.AI;
using UnityEngine;
using Interfaces;

namespace Enemy.Melee
{
    public class MeleeAI : MonoBehaviour, IDamageable
    {
        [SerializeField] private EnemyBalancer _enemyBalancer;
        [SerializeField] private GameObject _player;

        private NavMeshAgent _agent;
        private Animator _anim;
        private StateMachine _currentState;
        private NavMeshPath _path;
        private Events _state;
        private float _health;

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
            _health -= damage;

            _currentState.MeleeDamage();

            if(_health > 0)
            {
                _currentState.MeleeDamage();
            }
            else
            {
                _currentState.MeleeDeath();
            }
        }

        private void Start()
        {
            _anim = GetComponent<Animator>();
            _agent = GetComponent<NavMeshAgent>();
            _path = new NavMeshPath();
            _currentState = new MeleeSpawn(gameObject, _player, _agent, _anim, _path, _enemyBalancer);
            _health = _enemyBalancer.health;
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
