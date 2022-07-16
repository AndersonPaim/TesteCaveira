using UnityEngine;
using UnityEngine.AI;
using Cysharp.Threading.Tasks;

namespace Enemy.Archer
{
    public class ArcherSpawn : StateMachine
    {
        public ArcherSpawn(GameObject enemy, GameObject player, NavMeshAgent agent, Animator anim, NavMeshPath path, EnemyBalancer balancer)
                    : base(enemy, player, agent, anim, path, balancer)
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
            NextState = new ArcherMoving(Enemy, Player, Agent, Anim, Path, Balancer);
            Stage = Events.EXIT;
        }
    }
}