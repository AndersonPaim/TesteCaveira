using System;
using System.Threading;
using _Project.Scripts.Managers;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace _Project.Scripts.Enemies.States
{
	public class EnemyBlocking : StateMachine
	{
		public Action OnExit;
		public Action OnCancel;
		
		private Transform _targetPos;
		private float _stopDistance;
		private CancellationTokenSource _cancellationTokenSource;

		public EnemyBlocking(GameObject enemy, GameObject player, NavMeshAgent agent, SkinnedMeshRenderer mesh, Animator anim, EnemyBalancer balancer, WaypointController waypoints, Transform targetPos, float stopDistance)
			: base(enemy, player, agent, mesh, anim, balancer, waypoints)
		{
			CurrentState = EnemyStates.BLOCKING;
			_targetPos = targetPos;
			_stopDistance = stopDistance;
		}

		protected override void Enter()
		{
			Anim.SetTrigger("Blocking");
			StopBlockingDelay();
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
			_cancellationTokenSource?.Cancel();
		}
		
		private void Move()
		{
			Vector3 position = _targetPos.position;
			Agent.SetDestination(position);
			float targetDistance = Vector3.Distance(Enemy.transform.position, position);

			if(targetDistance < _stopDistance)
			{
				OnCancel?.Invoke();
			}
		}

		private async UniTask StopBlockingDelay()
		{
			_cancellationTokenSource = new CancellationTokenSource();
			await UniTask.Delay((int)(Balancer.blockDuration * 1000), cancellationToken: _cancellationTokenSource.Token);
			OnExit?.Invoke();
		}
	}
}