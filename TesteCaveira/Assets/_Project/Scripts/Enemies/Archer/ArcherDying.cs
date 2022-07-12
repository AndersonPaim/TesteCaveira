using UnityEngine;
using UnityEngine.AI;
using Cysharp.Threading.Tasks;

namespace Enemy.Archer
{
    public class ArcherDying : StateMachine
    {
        public ArcherDying(GameObject enemy, GameObject player, NavMeshAgent agent, Animator anim, NavMeshPath path)
                    : base(enemy, player, agent, anim, path)
        {
            currentState = States.DYING;
        }

        public override void Enter()
        {
            Anim.SetTrigger("Death");
            DisableObjectDelay();
            base.Enter();
        }

        private async UniTask DisableObjectDelay()
        {
            await UniTask.Delay(5000);
            Enemy.gameObject.SetActive(false);
        }
    }
}