using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy.Archer
{
    public class ArcherMoving : StateMachine
    {
        private Transform _waypoint;
        private EnemyBalancer _enemyBalancer;

        public ArcherMoving(GameObject enemy, GameObject player, NavMeshAgent agent, Animator anim, NavMeshPath path, EnemyBalancer enemyBalancer)
                    : base(enemy, player, agent, anim, path)
        {
            currentState = States.MOVING;
            _enemyBalancer = enemyBalancer;
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

            if(Agent.remainingDistance < Agent.stoppingDistance && Agent.remainingDistance != 0)
            {
                NextState = new ArcherIdle(Enemy, Player, Agent, Anim, Path, _enemyBalancer);
                Stage = Events.EXIT;
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