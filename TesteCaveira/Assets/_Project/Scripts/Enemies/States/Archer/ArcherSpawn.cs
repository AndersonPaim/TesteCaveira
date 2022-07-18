using UnityEngine;
using UnityEngine.AI;
using Cysharp.Threading.Tasks;
using Managers;

namespace Enemy.Archer
{
    public class ArcherSpawn : StateMachine
    {
        public ArcherSpawn(GameObject enemy, GameObject player, NavMeshAgent agent, Animator anim, NavMeshPath path, EnemyBalancer balancer, GameManager manager)
                    : base(enemy, player, agent, anim, path, balancer, manager)
        {
            CurrentState = States.SPAWNING;
        }

        public override void Enter()
        {
            Anim.SetTrigger("Spawn");
            SpawnDelay();
            base.Enter();
        }

        private async UniTask SpawnDelay()
        {
            await UniTask.Delay(1000);
            NextState = new ArcherMoving(Enemy, Player, Agent, Anim, Path, Balancer, Manager);
            Stage = Events.EXIT;
        }
    }
}