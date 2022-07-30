using System.Threading;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Managers;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Enemies.States.Melee
{
    public class MeleeAI : EnemyBase, IDamageable
    {
        private CancellationTokenSource _cancellationTokenSource;
        
        public override void TakeDamage(float damage)
        {
            if (CurrentState.CurrentState != EnemyStates.BLOCKING)
            {
                Health -= damage;
            }

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
            
            if(_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
            }
            
            BlockingState();
        }

        private async UniTask BlockingState()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            await UniTask.Delay((int)(EnemyBalancer.walkTime * 1000), cancellationToken: _cancellationTokenSource.Token);
            
            if (CurrentState.CurrentState == EnemyStates.MOVING)
            {
                EnemyBlocking blockingState = new EnemyBlocking(gameObject, Player, Agent, Mesh, Anim, EnemyBalancer, Waypoints, Player.transform, EnemyBalancer.attackDistance);
                blockingState.OnCancel += AttackState;
                blockingState.OnExit += MovingState;
                ChangeState(blockingState);
            }
        }

        private void ChangeState(StateMachine state)
        {
            CurrentState.StateMachineNextState = state;
            CurrentState.Stage = Events.EXIT;
        }
    }
}
