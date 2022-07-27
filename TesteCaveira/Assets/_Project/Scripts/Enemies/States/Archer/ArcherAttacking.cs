using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using Managers;
using System;

namespace Enemy.Archer
{
    public class ArcherAttacking : StateMachine
    {
        public Action OnExit;
        private Vector3 _rayPosition;
        private bool _canAttack;

        public ArcherAttacking(GameObject enemy, GameObject player, NavMeshAgent agent, SkinnedMeshRenderer mesh, Animator anim, NavMeshPath path, EnemyBalancer balancer, GameManager manager)
                    : base(enemy, player, agent, mesh, anim, path, balancer, manager)
        {
            CurrentState = States.ARCHER_ATTACKING;
        }

        public override void Enter()
        {
            base.Enter();
            BowAttack();
        }

        public override void Update()
        {
            base.Update();
            SearchPlayer();
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

        private async UniTask BowAttack()
        {
            Anim.SetTrigger("Attack");
            await UniTask.Delay(Balancer.attackCooldown * 1000);

            if(_canAttack)
            {
                BowAttack();
            }
        }
    }
}