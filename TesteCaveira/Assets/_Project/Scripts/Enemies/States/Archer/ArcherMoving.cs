using System;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy.Archer
{
    public class ArcherMoving : StateMachine
    {
        public Action OnExit;
        private Transform _waypoint;

        public ArcherMoving(GameObject enemy, GameObject player, NavMeshAgent agent, SkinnedMeshRenderer mesh, Animator anim, NavMeshPath path, EnemyBalancer balancer, GameManager manager)
                    : base(enemy, player, agent, mesh, anim, path, balancer, manager)
        {
            CurrentState = States.ARCHER_MOVING;
        }

        public override void Enter()
        {
            _waypoint = GetAvailableWaypoint();
            Anim.SetTrigger("Run");
            base.Enter();
        }

        public override void Update()
        {
            base.Update();
            Move();
        }

        private void Move()
        {
            Agent.SetDestination(_waypoint.position);

            if(Agent.remainingDistance < Agent.stoppingDistance && Agent.remainingDistance > 1)
            {
                OnExit?.Invoke();
            }
        }

        private Transform GetAvailableWaypoint()
        {
            Transform closestWaypoint = null;
            List<Transform> archerWaypoints = WaypointController.sInstance.ArcherWaypoint;
            float currentWaypointDistance;
            float closestWaypointDistance = 0;

            for (int i = 0; i < archerWaypoints.Count; i++)
            {
                currentWaypointDistance = Vector3.Distance(archerWaypoints[i].transform.position, Enemy.transform.position);

                bool isPathAvailable = Agent.CalculatePath(archerWaypoints[i].transform.position, Path);

                if (closestWaypointDistance == 0)
                {
                    if (isPathAvailable)
                    {
                        closestWaypoint = archerWaypoints[i].transform;
                        closestWaypointDistance = currentWaypointDistance;
                    }
                }
                else if (currentWaypointDistance < closestWaypointDistance)
                {
                    if (isPathAvailable)
                    {
                        closestWaypoint = archerWaypoints[i].transform;
                        closestWaypointDistance = currentWaypointDistance;
                    }
                }
            }

            return closestWaypoint;
        }
    }
}