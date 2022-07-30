using System;
using _Project.Scripts.Managers;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace _Project.Scripts.Enemies.States
{
    public class EnemyAttacking : StateMachine
    {
        public Action OnExit;
        private bool _canAttack;

        public EnemyAttacking(GameObject enemy, GameObject player, NavMeshAgent agent, SkinnedMeshRenderer mesh, Animator anim, EnemyBalancer balancer, WaypointController waypoints)
                    : base(enemy, player, agent, mesh, anim, balancer, waypoints)
        {
            CurrentState = EnemyStates.ATTACKING;
        }

        protected override void Enter()
        {
            Attack();
            base.Enter();
            _canAttack = true;
        }

        protected override void Update()
        {
            base.Update();
            SearchPlayer();
        }

        protected override void Exit()
        {
            base.Exit();
            _canAttack = false;
        }

        private void SearchPlayer()
        {
            float targetDistance = Vector3.Distance(Enemy.transform.position, Player.transform.position);

            if(targetDistance > Balancer.attackDistance)
            {
                _canAttack = false;
                LostPlayer();
            }
        }

        private void LostPlayer()
        {
            OnExit?.Invoke();
        }

        private async UniTask Attack()
        {
            Anim.SetTrigger("Attack");
            await UniTask.Delay(Balancer.attackCooldown * 1000);

            if(_canAttack)
            {
                Attack();
            }
        }
    }
}