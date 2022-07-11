using UnityEngine;
using UnityEngine.AI;

namespace Enemy.Archer
{
    public class ArcherIdle : StateMachine
    {
        private EnemyBalancer _enemyBalancer;

        public ArcherIdle(GameObject enemy, GameObject player, NavMeshAgent agent, Animator anim, NavMeshPath path, EnemyBalancer enemyBalancer)
                    : base(enemy, player, agent, anim, path)
        {
            currentState = States.IDLE;
            _enemyBalancer = enemyBalancer;
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

            if(CanSeePlayer(_enemyBalancer.attackDistance, _enemyBalancer.viewAngle))
            {
                FindPlayer();
            }
        }

        private void FindPlayer()
        {
            NextState = new ArcherAttack(Enemy, Player, Agent, Anim, Path, _enemyBalancer);
            Stage = Events.EXIT;
        }
    }
}