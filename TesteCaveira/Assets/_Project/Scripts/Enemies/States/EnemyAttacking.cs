using System;
using Cysharp.Threading.Tasks;
using Managers;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class EnemyAttacking : StateMachine
    {
        public Action OnExit;
        private bool _canAttack;

        public EnemyAttacking(GameObject enemy, GameObject player, NavMeshAgent agent, SkinnedMeshRenderer mesh, Animator anim, NavMeshPath path, EnemyBalancer balancer, GameManager manager)
                    : base(enemy, player, agent, mesh, anim, path, balancer, manager)
        {
            CurrentState = States.ATTACKING;
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