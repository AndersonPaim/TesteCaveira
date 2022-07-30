using UnityEngine;
using UnityEngine.AI;
using Managers;
using System;
using Cysharp.Threading.Tasks;

namespace Enemy
{
	public class EnemyBlocking : StateMachine
	{
		public Action OnExit;
		public Action OnCancel;
		
		private Transform _targetPos;
		private float _stopDistance;

		public EnemyBlocking(GameObject enemy, GameObject player, NavMeshAgent agent, SkinnedMeshRenderer mesh, Animator anim, EnemyBalancer balancer, WaypointController waypoints, Transform targetPos, float stopDistance)
			: base(enemy, player, agent, mesh, anim, balancer, waypoints)
		{
			CurrentState = States.BLOCKING;
			_targetPos = targetPos;
			_stopDistance = stopDistance;
		}

		protected override void Enter()
		{
			Anim.SetTrigger("Blocking");
			base.Enter();
		}

		protected override void Update()
		{
			base.Update();
			Move();
			StopBlockingDelay();
		}
		
		private void Move()
		{
			Agent.SetDestination(_targetPos.position);

			float targetDistance = Vector3.Distance(Enemy.transform.position, _targetPos.position);

			if(targetDistance < _stopDistance)
			{
				OnCancel?.Invoke();
			}
		}

		private async UniTask StopBlockingDelay()
		{
			await UniTask.Delay((int)(Balancer.blockDuration * 1000));
			OnExit?.Invoke();
		}
	}
}