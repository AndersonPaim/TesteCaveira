using System;
using Managers;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy.Archer
{
    public class ArcherIdle : StateMachine
    {
        public Action OnExit;

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

            float targetDistance = Vector3.Distance(Enemy.transform.position, Player.transform.position);

            if(targetDistance <= Balancer.attackDistance)
            {
                FindPlayer();
            }
        }

        private void FindPlayer()
        {
            OnExit?.Invoke();
        }
    }
}