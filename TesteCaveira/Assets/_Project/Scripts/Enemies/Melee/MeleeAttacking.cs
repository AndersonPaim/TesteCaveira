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
            Attack();
            base.Enter();
            _canAttack = true;
        }

        public override void Update()
        {
            base.Update();
            SearchPlayer();
        }

        public override void Exit()
        {
            base.Exit();
            _canAttack = false;
        }

        private void SearchPlayer()
        {
            float targetDistance = Vector3.Distance(Enemy.transform.position, Player.transform.position);

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
                Attack();
            }
        }
    }
}