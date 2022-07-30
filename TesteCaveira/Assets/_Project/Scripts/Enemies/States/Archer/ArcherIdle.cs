using System;
using _Project.Scripts.Managers;
using UnityEngine;
using UnityEngine.AI;

namespace _Project.Scripts.Enemies.States.Archer
{
    public class ArcherIdle : StateMachine
    {
        public Action OnExit;

        public ArcherIdle(GameObject enemy, GameObject player, NavMeshAgent agent, SkinnedMeshRenderer mesh, Animator anim, EnemyBalancer balancer, WaypointController waypoints)
                    : base(enemy, player, agent, mesh, anim, balancer, waypoints)
        {
            CurrentState = EnemyStates.ARCHER_IDLE;
        }

        protected override void Enter()
        {
            Anim.SetTrigger("Idle");
            base.Enter();
        }

        protected override void Update()
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