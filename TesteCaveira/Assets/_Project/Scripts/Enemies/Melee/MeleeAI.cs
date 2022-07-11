using UnityEngine.AI;
using UnityEngine;

namespace Enemy.Melee
{
    public class MeleeAI : MonoBehaviour
    {
        [SerializeField] private EnemyBalancer _enemyBalancer;
        [SerializeField] private GameObject _player;

        private NavMeshAgent _agent;
        private Animator _anim;
        private StateMachine _currentState;
        private NavMeshPath _path;

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

        private void Start()
        {
            _anim = GetComponent<Animator>();
            _agent = GetComponent<NavMeshAgent>();
            _path = new NavMeshPath();
            _currentState = new MeleeSpawn(gameObject, _player, _agent, _anim, _path, _enemyBalancer);
        }

        private void Update()
        {
            _currentState = _currentState.Process();
        }
    }
}
