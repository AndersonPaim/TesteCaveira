using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy.Melee
{
    public class MeleeMoving : StateMachine
    {
        private EnemyBalancer _enemyBalancer;

        public MeleeMoving(GameObject enemy, GameObject player, NavMeshAgent agent, Animator anim, NavMeshPath path, EnemyBalancer enemyBalancer)
                    : base(enemy, player, agent, anim, path)
        {
            currentState = States.MOVING;
            _enemyBalancer = enemyBalancer;
        }

        public override void Enter()
        {
            Anim.SetBool("Run", true);
            base.Enter();
        }

        public override void Update()
        {
            base.Update();
            Debug.Log("MOVING");
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

            if(targetDistance < _enemyBalancer.attackDistance)
            {
                NextState = new MeleeAttack(Enemy, Player, Agent, Anim, Path, _enemyBalancer);
                Stage = Events.EXIT;
            }
        }
    }
}