using UnityEngine;
using UnityEngine.AI;
using Cysharp.Threading.Tasks;
using System;
using Managers;

namespace Enemy
{
    public class EnemyDamage : StateMachine
    {
        public Action OnExit;

        public EnemyDamage(GameObject enemy, GameObject player, NavMeshAgent agent, SkinnedMeshRenderer mesh, Animator anim, NavMeshPath path, EnemyBalancer balancer, GameManager manager)
                    : base(enemy, player, agent, mesh, anim, path, balancer, manager)
        {
            CurrentState = States.TAKINGDAMAGE;
        }

        protected override void Enter()
        {
            Anim.SetTrigger("TakeDamage");
            Agent.SetDestination(Enemy.transform.position);
            StunDelayASync();
            base.Enter();
        }

        private async UniTask StunDelayASync()
        {
            await UniTask.Delay(Balancer.stunCooldown * 1000);
            OnExit?.Invoke();
        }
    }
}