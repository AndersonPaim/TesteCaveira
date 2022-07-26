using UnityEngine;
using UnityEngine.AI;
using Interfaces;
using Managers;

namespace Enemy.Archer
{
    public class ArcherAI : EnemyBase, IDamageable
    {
        [SerializeField] private Transform _shootPosition;
        [SerializeField] private GameObject _arrowPrefab;

        private EnemySpawn _spawnState;
        private ArcherMoving _movingState;
        private ArcherIdle _idleState;
        private ArcherAttacking _attackingState;
        private EnemyDamage _damageState;

        public void Attack()
        {
            GameObject arrow = Manager.ObjectPooler.SpawnFromPool(_arrowPrefab.GetInstanceID());
            arrow.transform.position =  _shootPosition.position;
            Rigidbody rb = arrow.GetComponent<Rigidbody>();
            rb.velocity = _shootPosition.transform.forward * EnemyBalancer.shootForce;
        }

        public override void SetupEnemy(GameManager manager)
        {
            base.SetupEnemy(manager);
            _spawnState = new EnemySpawn(gameObject, Player, Agent, Mesh, Anim, Path, EnemyBalancer, manager);
            _movingState = new ArcherMoving(gameObject, Player, Agent, Mesh, Anim, Path, EnemyBalancer, manager);
            _idleState = new ArcherIdle(gameObject, Player, Agent, Mesh, Anim, Path, EnemyBalancer, manager);
            _attackingState = new ArcherAttacking(gameObject, Player, Agent, Mesh, Anim, Path, EnemyBalancer, manager);
            _damageState = new EnemyDamage(gameObject, Player, Agent, Mesh, Anim, Path, EnemyBalancer, Manager);
            _spawnState.NextState = _movingState;
            _movingState.NextState = _idleState;
            _idleState.NextState = _attackingState;
            _attackingState.NextState = _idleState;
            CurrentState = _spawnState;
        }

        public override void TakeDamage(float damage)
        {
            if(CurrentState.CurrentState == States.ARCHER_MOVING)
            {
                _damageState.NextState = _movingState;
            }
            else
            {
                _damageState.NextState = _idleState;
            }

            base.TakeDamage(damage);

            CurrentState.StateMachineNextState = _damageState;
            CurrentState.Stage = Events.EXIT;
        }
    }
}

