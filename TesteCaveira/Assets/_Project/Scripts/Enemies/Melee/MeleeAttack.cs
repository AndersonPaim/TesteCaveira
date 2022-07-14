using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy.Melee
{
    public class MeleeAttacking : StateMachine
    {
        private Vector3 _rayPosition;
        private bool _canAttack;

        public MeleeAttacking(GameObject enemy, GameObject player, NavMeshAgent agent, Animator anim, NavMeshPath path, EnemyBalancer balancer)
                    : base(enemy, player, agent, anim, path, balancer)
        {
            CurrentState = States.ATTACKING;
        }

        public override void Enter()
        {
            base.Enter();
            _canAttack = true;
            Attack();
        }

        public override void Update()
        {
            base.Update();
            SearchPlayer();
        }

        private void SearchPlayer()
        {
            float targetDistance = Vector3.Distance(Enemy.transform.position, Player.transform.position);

            Enemy.transform.LookAt(Player.transform);

            if(targetDistance > Balancer.attackDistance)
            {
                _canAttack = false;
                LostPlayer();
            }
        }

        private void LostPlayer()
        {
            NextState = new MeleeMoving(Enemy, Player, Agent, Anim, Path, Balancer);
            Stage = Events.EXIT;
        }

        private async UniTask Attack()
        {
            Anim.SetTrigger("Attack");
            await UniTask.Delay(Balancer.attackCooldown * 1000);

            if(_canAttack)
            {
                await Attack();
            }
        }
    }
}