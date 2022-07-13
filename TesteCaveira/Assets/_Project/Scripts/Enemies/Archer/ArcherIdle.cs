using UnityEngine;
using UnityEngine.AI;

namespace Enemy.Archer
{
    public class ArcherIdle : StateMachine
    {
        public ArcherIdle(GameObject enemy, GameObject player, NavMeshAgent agent, Animator anim, NavMeshPath path, EnemyBalancer enemyBalancer)
                    : base(enemy, player, agent, anim, path, enemyBalancer)
        {
            CurrentState = States.IDLE;
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
            Enemy.transform.LookAt(Player.transform);

            if(CanSeePlayer(Balancer.attackDistance, Balancer.viewAngle))
            {
                FindPlayer();
            }
        }

        private void FindPlayer()
        {
            NextState = new ArcherAttacking(Enemy, Player, Agent, Anim, Path, Balancer);
            Stage = Events.EXIT;
        }
    }
}