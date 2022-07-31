using System.Collections.Generic;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Managers;
using UnityEngine;

namespace _Project.Scripts.Enemies.States.Archer
{
    public class ArcherAI : EnemyBase, IDamageable
    {
        [SerializeField] private Transform _shootPosition;
        [SerializeField] private GameObject _arrowPrefab;

        public void Attack()
        {
            GameObject arrow = ObjectPooler.sInstance.SpawnFromPool(_arrowPrefab.GetInstanceID());
            arrow.transform.position =  _shootPosition.position;
            Rigidbody rb = arrow.GetComponent<Rigidbody>();
            rb.velocity = _shootPosition.transform.forward * EnemyBalancer.shootForce;
        }

        public override void SetupEnemy(WaypointController waypoints, GameObject player)
        {
            base.SetupEnemy(waypoints, player);
            EnemySpawn spawnState = new EnemySpawn(gameObject, Player, Agent, Mesh, Anim, EnemyBalancer, waypoints);
            spawnState.OnExit += MovingState;
            CurrentState = spawnState;
        }

        public override void TakeDamage(float damage)
        {
            Health -= damage;

            if(Health > 0)
            {
                DamageState();
            }
            else
            {
                DyingState();
            }

            base.TakeDamage(damage);
        }

        private void DamageState()
        {
            EnemyDamage damageState = new EnemyDamage(gameObject, Player, Agent, Mesh, Anim, EnemyBalancer, Waypoints);
            damageState.OnExit += MovingState;
            ChangeState(damageState);
        }

        private void DyingState()
        {
            EnemyDying dyingState = new EnemyDying(gameObject, Player, Agent, Mesh, Anim, EnemyBalancer, Waypoints);
            ChangeState(dyingState);
        }

        private void AttackState()
        {
            EnemyAttacking attackingState = new EnemyAttacking(gameObject, Player, Agent, Mesh, Anim, EnemyBalancer, Waypoints);
            attackingState.OnExit += IdleState;
            ChangeState(attackingState);
        }

        private void IdleState()
        {
            ArcherIdle idleState = new ArcherIdle(gameObject, Player, Agent, Mesh, Anim, EnemyBalancer, Waypoints);
            idleState.OnExit += AttackState;
            ChangeState(idleState);
        }

        private void MovingState()
        {
            EnemyMoving movingState = new EnemyMoving(gameObject, Player, Agent, Mesh, Anim, EnemyBalancer, Waypoints, GetAvailableWaypoint(), Agent.stoppingDistance);
            movingState.OnExit += IdleState;
            ChangeState(movingState);
        }

        private void ChangeState(StateMachine state)
        {
            CurrentState.StateMachineNextState = state;
            CurrentState.Stage = Events.EXIT;
        }

        private Transform GetAvailableWaypoint()
        {
            Transform closestWaypoint = null;
            List<Transform> archerWaypoints = Waypoints.ArcherWaypoint;
            float currentWaypointDistance;
            float closestWaypointDistance = 0;

            foreach (Transform t in archerWaypoints)
            {
                Vector3 position = t.transform.position;
                currentWaypointDistance = Vector3.Distance(position, gameObject.transform.position);

                bool isPathAvailable = Agent.CalculatePath(position, Path);

                if (closestWaypointDistance == 0)
                {
                    if (isPathAvailable && currentWaypointDistance > Agent.stoppingDistance)
                    {
                        closestWaypoint = t.transform;
                        closestWaypointDistance = currentWaypointDistance;
                    }
                }
                else if (currentWaypointDistance < closestWaypointDistance && currentWaypointDistance > Agent.stoppingDistance)
                {
                    if (isPathAvailable)
                    {
                        closestWaypoint = t.transform;
                        closestWaypointDistance = currentWaypointDistance;
                    }
                }
            }

            return closestWaypoint;
        }
    }
}

