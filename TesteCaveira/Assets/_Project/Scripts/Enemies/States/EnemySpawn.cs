using UnityEngine;
using UnityEngine.AI;
using Cysharp.Threading.Tasks;
using Managers;
using Enemy.Archer;
using Enemy.Melee;

namespace Enemy
{
    public class EnemySpawn : StateMachine
    {
        public StateMachine NextState;

        public EnemySpawn(GameObject enemy, GameObject player, NavMeshAgent agent, SkinnedMeshRenderer mesh, Animator anim, NavMeshPath path, EnemyBalancer balancer, GameManager manager)
                    : base(enemy, player, agent, mesh, anim, path, balancer, manager)
        {
            CurrentState = States.SPAWNING;
        }

        public override void Enter()
        {
            SetRandomTexture();
            Anim.SetTrigger("Spawn");
            SpawnDelay();
            base.Enter();
        }

        private void SetRandomTexture()
        {
            int randomValue = Random.Range(0, Balancer.textures.Count);
            Mesh.material = Balancer.textures[randomValue];
        }

        private async UniTask SpawnDelay()
        {
            await UniTask.Delay(1000);

            StateMachineNextState = NextState;
            //NextState = new MeleeMoving(Enemy, Player, Agent, Mesh, Anim, Path, Balancer, Manager);

            Stage = Events.EXIT;
        }
    }
}