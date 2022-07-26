using UnityEngine;
using UnityEngine.AI;
using Managers;

namespace Enemy.Melee
{
    public class MeleeMoving : StateMachine
    {
        public MeleeMoving(GameObject enemy, GameObject player, NavMeshAgent agent, SkinnedMeshRenderer mesh, Animator anim, NavMeshPath path, EnemyBalancer balancer, GameManager manager)
                    : base(enemy, player, agent, mesh, anim, path, balancer, manager)
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
                StateMachineNextState = new MeleeAttacking(Enemy, Player, Agent, Mesh, Anim, Path, Balancer, Manager);
                Stage = Events.EXIT;
            }
        }
    }
}