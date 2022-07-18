using UnityEngine;
using UnityEngine.AI;
using Managers;

namespace Enemy.Melee
{
    public class MeleeMoving : StateMachine
    {
        public MeleeMoving(GameObject enemy, GameObject player, NavMeshAgent agent, Animator anim, NavMeshPath path, EnemyBalancer balancer, Managers.GameManager manager)
                    : base(enemy, player, agent, anim, path, balancer, manager)
        {
            CurrentState = States.MELEE_MOVING;
        }

        public override void Enter()
        {
            Anim.SetBool("Run", true);
            base.Enter();
        }

        public override void Update()
        {
            base.Update();
            Move();
        }

        public override void Exit()
        {
            base.Exit();
            Anim.SetBool("Run", false);
        }

        private void Move()
        {
            Agent.SetDestination(Player.transform.position);

            float targetDistance = Vector3.Distance(Enemy.transform.position, Player.transform.position);

            if(targetDistance < Balancer.attackDistance)
            {
                NextState = new MeleeAttacking(Enemy, Player, Agent, Anim, Path, Balancer, Manager);
                Stage = Events.EXIT;
            }
        }
    }
}