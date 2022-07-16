using UnityEngine;
using UnityEngine.AI;
using Cysharp.Threading.Tasks;

namespace Enemy.Melee
{
    public class EnemyDying : StateMachine
    {
        private int _destroyDelay;

        public EnemyDying(GameObject enemy, GameObject player, NavMeshAgent agent, Animator anim, NavMeshPath path, EnemyBalancer balancer)
                    : base(enemy, player, agent, anim, path, balancer)
        {
            CurrentState = States.DYING;
            _destroyDelay = balancer.destroyDelay;
        }

        public override void Enter()
        {
            Agent.enabled = false;
            Anim.SetTrigger("Death");
            DisableObjectASync();
            base.Enter();
        }

        private async UniTask DisableObjectASync()
        {
            await UniTask.Delay(_destroyDelay * 1000);
            Enemy.gameObject.SetActive(false);
        }
    }
}