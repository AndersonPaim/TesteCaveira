using UnityEngine;
using UnityEngine.AI;
using Cysharp.Threading.Tasks;

namespace Enemy.Melee
{
    public class MeleeSpawn : StateMachine
    {
        private EnemyBalancer _enemyBalancer;

        public MeleeSpawn(GameObject enemy, GameObject player, NavMeshAgent agent, Animator anim, NavMeshPath path, EnemyBalancer archerBalancer)
                    : base(enemy, player, agent, anim, path)
        {
            currentState = States.SPAWNING;
            _enemyBalancer = archerBalancer;
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
            NextState = new MeleeMoving(Enemy, Player, Agent, Anim, Path, _enemyBalancer);
            Stage = Events.EXIT;
        }
    }
}