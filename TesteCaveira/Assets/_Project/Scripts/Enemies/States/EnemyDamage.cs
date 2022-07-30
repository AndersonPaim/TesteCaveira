using System;
using _Project.Scripts.Managers;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace _Project.Scripts.Enemies.States
{
    public class EnemyDamage : StateMachine
    {
        public Action OnExit;

        public EnemyDamage(GameObject enemy, GameObject player, NavMeshAgent agent, SkinnedMeshRenderer mesh, Animator anim, EnemyBalancer balancer, WaypointController waypoints)
                    : base(enemy, player, agent, mesh, anim, balancer, waypoints)
        {
            CurrentState = EnemyStates.TAKINGDAMAGE;
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