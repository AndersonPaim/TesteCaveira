using UnityEngine;
using UnityEngine.AI;
using Cysharp.Threading.Tasks;

namespace Enemy.Archer
{
    public class ArcherDamage : StateMachine
    {
        public ArcherDamage(GameObject enemy, GameObject player, NavMeshAgent agent, Animator anim, NavMeshPath path, EnemyBalancer balancer)
                    : base(enemy, player, agent, anim, path, balancer)
        {
            CurrentState = States.TAKINGDAMAGE;
        }

        public override void Enter()
        {
            Anim.SetTrigger("TakeDamage");
            StunDelayASync();
            base.Enter();
        }

        private async UniTask StunDelayASync()
        {
            await UniTask.Delay(Balancer.stunCooldown * 1000);

            switch(LastState)
            {
                case States.MOVING:
                    NextState = new ArcherMoving(Enemy, Player, Agent, Anim, Path, Balancer);
                    break;
                case States.IDLE:
                    NextState = new ArcherIdle(Enemy, Player, Agent, Anim, Path, Balancer);
                    break;
            }

            Stage = Events.EXIT;
        }
    }
}