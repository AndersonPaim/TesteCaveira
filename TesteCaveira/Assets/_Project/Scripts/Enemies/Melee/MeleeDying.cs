using UnityEngine;
using UnityEngine.AI;
using Cysharp.Threading.Tasks;

namespace Enemy.Melee
{
    public class MeleeDying : StateMachine
    {
        public MeleeDying(GameObject enemy, GameObject player, NavMeshAgent agent, Animator anim, NavMeshPath path, EnemyBalancer balancer)
                    : base(enemy, player, agent, anim, path, balancer)
        {
            CurrentState = States.DYING;
        }

        public override void Enter()
        {
            Anim.SetTrigger("Death");
            DisableObjectASync();
            base.Enter();
        }

        private async UniTask DisableObjectASync()
        {
            await UniTask.Delay(3000);
            Enemy.gameObject.SetActive(false);
        }
    }
}