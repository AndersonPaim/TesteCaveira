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
        private States _lastState;

        public EnemyDamage(GameObject enemy, GameObject player, NavMeshAgent agent, Animator anim, NavMeshPath path, EnemyBalancer balancer, States lastState, GameManager manager)
                    : base(enemy, player, agent, anim, path, balancer, manager)
        {
            CurrentState = States.TAKINGDAMAGE;
            _lastState = lastState;
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

            switch(_lastState)
            {
                case States.ARCHER_IDLE:
                {
                    NextState = new ArcherIdle(Enemy, Player, Agent, Anim, Path, Balancer, Manager);
                    break;
                }
                case States.ARCHER_MOVING:
                {
                    NextState = new ArcherMoving(Enemy, Player, Agent, Anim, Path, Balancer, Manager);
                    break;
                }
                case States.ARCHER_ATTACKING:
                {
                    NextState = new ArcherIdle(Enemy, Player, Agent, Anim, Path, Balancer, Manager);
                    break;
                }
                default:
                {
                    NextState = new MeleeMoving(Enemy, Player, Agent, Anim, Path, Balancer, Manager);
                    break;
                }
            }

            Stage = Events.EXIT;
        }
    }
}