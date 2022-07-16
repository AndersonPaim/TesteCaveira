using UnityEngine;
using UnityEngine.AI;
using Cysharp.Threading.Tasks;

namespace Enemy.Melee
{
    public class MeleeSpawn : StateMachine
    {
        public MeleeSpawn(GameObject enemy, GameObject player, NavMeshAgent agent, Animator anim, NavMeshPath path, EnemyBalancer balancer)
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
            NextState = new MeleeMoving(Enemy, Player, Agent, Anim, Path, Balancer);
            Stage = Events.EXIT;
        }
    }
}