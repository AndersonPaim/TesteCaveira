using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy.Archer
{
    public class ArcherAttacking : StateMachine
    {
        private Vector3 _rayPosition;
        private bool _canAttack;

        public ArcherAttacking(GameObject enemy, GameObject player, NavMeshAgent agent, Animator anim, NavMeshPath path, EnemyBalancer enemyBalancer)
                    : base(enemy, player, agent, anim, path, enemyBalancer)
        {
            CurrentState = States.ATTACKING;
        }

        public override void Enter()
        {
            base.Enter();
            BowAttack();
        }

        public override void Update()
        {
            base.Update();
            SearchPlayer();
        }

        private void SearchPlayer()
        {
            if(CanSeePlayer(Balancer.attackDistance, Balancer.viewAngle))
            {
                _canAttack = true;
                Enemy.transform.LookAt(Player.transform);
            }
            else
            {
                _canAttack = false;
                LostPlayer();
            }
        }

        private void LostPlayer()
        {
            NextState = new ArcherIdle(Enemy, Player, Agent, Anim, Path, Balancer);
            Stage = Events.EXIT;
        }

        private async UniTask BowAttack()
        {
            Anim.SetTrigger("Attack");
            await UniTask.Delay(Balancer.attackCooldown * 1000);

            if(_canAttack)
            {
                await BowAttack();
            }
        }
    }
}