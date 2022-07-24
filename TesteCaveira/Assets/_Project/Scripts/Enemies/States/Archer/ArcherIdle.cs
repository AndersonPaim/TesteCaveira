using Managers;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy.Archer
{
    public class ArcherIdle : StateMachine
    {
        public ArcherIdle(GameObject enemy, GameObject player, NavMeshAgent agent, SkinnedMeshRenderer mesh, Animator anim, NavMeshPath path, EnemyBalancer balancer, GameManager manager)
                    : base(enemy, player, agent, mesh, anim, path, balancer, manager)
        {
            CurrentState = States.ARCHER_IDLE;
        }

        public override void Enter()
        {
            Anim.SetTrigger("Idle");
            base.Enter();
        }

        public override void Update()
        {
            base.Update();
            SearchPlayer();
        }

        private void SearchPlayer()
        {
            Vector3 target = new Vector3(Player.transform.position.x, Enemy.transform.position.y, Player.transform.position.z);
            Enemy.transform.LookAt(target);

            if(CanSeePlayer(Balancer.attackDistance, Balancer.viewAngle))
            {
                FindPlayer();
            }
        }

        private void FindPlayer()
        {
            NextState = new ArcherAttacking(Enemy, Player, Agent, Mesh, Anim, Path, Balancer, Manager);
            Stage = Events.EXIT;
        }
    }
}