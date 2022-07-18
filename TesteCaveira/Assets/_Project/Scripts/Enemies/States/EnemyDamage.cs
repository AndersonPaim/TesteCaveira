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
        private Enemies _enemy;

        public EnemyDamage(GameObject enemy, GameObject player, NavMeshAgent agent, Animator anim, NavMeshPath path, EnemyBalancer balancer, Enemies enemyType, GameManager manager)
                    : base(enemy, player, agent, anim, path, balancer, manager)
        {
            CurrentState = States.TAKINGDAMAGE;
            _enemy = enemyType;
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

            if(_enemy == Enemies.MELEE)
            {
                NextState = new MeleeMoving(Enemy, Player, Agent, Anim, Path, Balancer, Manager);
            }
            else
            {
                if(LastState == States.ARCHER_MOVING)
                {
                    NextState = new ArcherMoving(Enemy, Player, Agent, Anim, Path, Balancer, Manager);
                }
                else
                {
                    NextState = new ArcherIdle(Enemy, Player, Agent, Anim, Path, Balancer, Manager);
                }
            }

            Stage = Events.EXIT;
        }
    }
}