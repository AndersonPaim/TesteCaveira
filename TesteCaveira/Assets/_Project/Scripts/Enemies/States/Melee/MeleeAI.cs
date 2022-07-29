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
            EnemyDamage damageState = new EnemyDamage(gameObject, Player, Agent, Mesh, Anim, EnemyBalancer, Waypoints);
            damageState.OnExit += MovingState;
            ChangeState(damageState);
        }

        public override void SetupEnemy(WaypointController waypoints, GameObject player)
        {
            base.SetupEnemy(waypoints, player);
            EnemySpawn spawnState = new EnemySpawn(gameObject, Player, Agent, Mesh, Anim, EnemyBalancer, Waypoints);
            spawnState.OnExit += MovingState;
            CurrentState = spawnState;
        }

        private void DyingState()
        {
            EnemyDying dyingState = new EnemyDying(gameObject, Player, Agent, Mesh, Anim, EnemyBalancer, Waypoints);
            ChangeState(dyingState);
        }

        private void AttackState()
        {
            EnemyAttacking attackingState = new EnemyAttacking(gameObject, Player, Agent, Mesh, Anim, EnemyBalancer, Waypoints);
            attackingState.OnExit += MovingState;
            ChangeState(attackingState);
        }

        private void MovingState()
        {
            EnemyMoving movingState = new EnemyMoving(gameObject, Player, Agent, Mesh, Anim, EnemyBalancer, Waypoints, Player.transform, EnemyBalancer.attackDistance);
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
