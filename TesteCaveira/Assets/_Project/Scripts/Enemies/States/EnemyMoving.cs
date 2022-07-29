using UnityEngine;
using UnityEngine.AI;
using Managers;
using System;

namespace Enemy
{
    public class EnemyMoving : StateMachine
    {
        public Action OnExit;
        private Transform _targetPos;
        private float _stopDistance;

        public EnemyMoving(GameObject enemy, GameObject player, NavMeshAgent agent, SkinnedMeshRenderer mesh, Animator anim, NavMeshPath path, EnemyBalancer balancer, GameManager manager, Transform targetPos, float stopDistance)
                    : base(enemy, player, agent, mesh, anim, path, balancer, manager)
        {
            CurrentState = States.MOVING;
            _targetPos = targetPos;
            _stopDistance = stopDistance;
        }

        protected override void Enter()
        {
            Anim.SetBool("Run", true);
            base.Enter();
        }

        protected override void Update()
        {
            base.Update();
            Move();
        }

        protected override void Exit()
        {
            base.Exit();
            Anim.SetBool("Run", false);
        }

        private void Move()
        {
            Agent.SetDestination(_targetPos.position);

            float targetDistance = Vector3.Distance(Enemy.transform.position, _targetPos.position);

            if(targetDistance < _stopDistance)
            {
                OnExit?.Invoke();
            }
        }
    }
}