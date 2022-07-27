using UnityEngine;
using UnityEngine.AI;
using Interfaces;
using Managers;
using Enemy.Melee;

namespace Enemy.Archer
{
    public class ArcherAI : EnemyBase, IDamageable
    {
        [SerializeField] private Transform _shootPosition;
        [SerializeField] private GameObject _arrowPrefab;

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
            EnemySpawn spawnState = new EnemySpawn(gameObject, Player, Agent, Mesh, Anim, Path, EnemyBalancer, manager);
            spawnState.OnExit += MovingState;
            CurrentState = spawnState;
        }

        public override void TakeDamage(float damage)
        {
            Health -= damage;

            if(Health > 0)
            {
                DamageState();
            }
            else
            {
                DyingState();
            }

            base.TakeDamage(damage);
        }

        private void DamageState()
        {
            EnemyDamage damageState = new EnemyDamage(gameObject, Player, Agent, Mesh, Anim, Path, EnemyBalancer, Manager);

            if(CurrentState.CurrentState == States.ARCHER_MOVING)
            {
                damageState.OnExit += MovingState;
            }
            else
            {
                damageState.OnExit += IdleState;
            }

            CurrentState.StateMachineNextState = damageState;
            CurrentState.Stage = Events.EXIT;
        }

        private void DyingState()
        {
            EnemyDying dyingState = new EnemyDying(gameObject, Player, Agent, Mesh, Anim, Path, EnemyBalancer, Manager);
            CurrentState.StateMachineNextState = dyingState;
            CurrentState.Stage = Events.EXIT;
        }

        private void AttackState()
        {
            ArcherAttacking attackingState = new ArcherAttacking(gameObject, Player, Agent, Mesh, Anim, Path, EnemyBalancer, Manager);
            attackingState.OnExit += IdleState;
            CurrentState.StateMachineNextState = attackingState;
            CurrentState.Stage = Events.EXIT;
        }

        private void IdleState()
        {
            ArcherIdle idleState = new ArcherIdle(gameObject, Player, Agent, Mesh, Anim, Path, EnemyBalancer, Manager);
            idleState.OnExit += AttackState;
            CurrentState.StateMachineNextState = idleState;
            CurrentState.Stage = Events.EXIT;
        }

        private void MovingState()
        {
            ArcherMoving movingState = new ArcherMoving(gameObject, Player, Agent, Mesh, Anim, Path, EnemyBalancer, Manager);
            movingState.OnExit += AttackState;
            CurrentState.StateMachineNextState = movingState;
            CurrentState.Stage = Events.EXIT;
        }
    }
}

