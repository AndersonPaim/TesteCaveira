using UnityEngine;
using UnityEngine.AI;
using Cysharp.Threading.Tasks;
using System;
using Enemy.Melee;
using Enemy.Archer;
using Managers;

namespace Enemy
{
    public class EnemyDamage : StateMachine
    {
        public StateMachine NextState;

        public EnemyDamage(GameObject enemy, GameObject player, NavMeshAgent agent, SkinnedMeshRenderer mesh, Animator anim, NavMeshPath path, EnemyBalancer balancer, GameManager manager)
                    : base(enemy, player, agent, mesh, anim, path, balancer, manager)
        {
            CurrentState = States.TAKINGDAMAGE;
        }

        public override void Enter()
        {
            Anim.SetTrigger("TakeDamage");
            Agent.SetDestination(Enemy.transform.position);
            StunDelayASync();
            base.Enter();
        }

        private async UniTask StunDelayASync()
        {
            await UniTask.Delay(Balancer.stunCooldown * 1000);

            StateMachineNextState = NextState;
            Stage = Events.EXIT;
        }
    }
}