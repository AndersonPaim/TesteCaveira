using UnityEngine.AI;
using UnityEngine;
using Interfaces;
using Managers;

namespace Enemy.Melee
{
    public class MeleeAI : EnemyBase, IDamageable
    {
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
            damageState.OnExit += MovingState;
            ChangeState(damageState);
        }

        public override void SetupEnemy(GameManager manager)
        {
            base.SetupEnemy(manager);
            EnemySpawn spawnState = new EnemySpawn(gameObject, Player, Agent, Mesh, Anim, Path, EnemyBalancer, manager);
            spawnState.OnExit += MovingState;
            CurrentState = spawnState;
        }

        private void DyingState()
        {
            EnemyDying dyingState = new EnemyDying(gameObject, Player, Agent, Mesh, Anim, Path, EnemyBalancer, Manager);
            ChangeState(dyingState);
        }

        private void AttackState()
        {
            EnemyAttacking attackingState = new EnemyAttacking(gameObject, Player, Agent, Mesh, Anim, Path, EnemyBalancer, Manager);
            attackingState.OnExit += MovingState;
            ChangeState(attackingState);
        }

        private void MovingState()
        {
            EnemyMoving movingState = new EnemyMoving(gameObject, Player, Agent, Mesh, Anim, Path, EnemyBalancer, Manager, Player.transform, EnemyBalancer.attackDistance);
            movingState.OnExit += AttackState;
            ChangeState(movingState);
        }

        private void ChangeState(StateMachine state)
        {
            CurrentState.StateMachineNextState = state;
            CurrentState.Stage = Events.EXIT;
        }
    }
}
