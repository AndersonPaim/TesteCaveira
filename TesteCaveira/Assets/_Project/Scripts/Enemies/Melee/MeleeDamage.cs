using UnityEngine;
using UnityEngine.AI;
using Cysharp.Threading.Tasks;

namespace Enemy.Melee
{
    public class MeleeDamage : StateMachine
    {
        public MeleeDamage(GameObject enemy, GameObject player, NavMeshAgent agent, Animator anim, NavMeshPath path, EnemyBalancer balancer)
                    : base(enemy, player, agent, anim, path, balancer)
        {
            CurrentState = States.TAKINGDAMAGE;
        }

        public override void Enter()
        {
            base.Enter();
            Anim.SetTrigger("TakeDamage");
            StunDelayASync();
        }

        private async UniTask StunDelayASync()
        {
            Agent.SetDestination(Enemy.transform.position);
            await UniTask.Delay(Balancer.stunCooldown * 1000);
            NextState = new MeleeMoving(Enemy, Player, Agent, Anim, Path, Balancer);
            Stage = Events.EXIT;
        }
    }
}