using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy.Archer
{
    public class ArcherAttack : StateMachine
    {
        private Vector3 _rayPosition;
        private EnemyBalancer _enemyBalancer;
        private bool _canAttack;

        public ArcherAttack(GameObject enemy, GameObject player, NavMeshAgent agent, Animator anim, NavMeshPath path, EnemyBalancer enemyBalancer)
                    : base(enemy, player, agent, anim, path)
        {
            currentState = States.ATTACK;
            _enemyBalancer = enemyBalancer;
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
            if(CanSeePlayer(_enemyBalancer.attackDistance, _enemyBalancer.viewAngle))
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
            NextState = new ArcherIdle(Enemy, Player, Agent, Anim, Path, _enemyBalancer);
            Stage = Events.EXIT;
        }

        private async UniTask BowAttack()
        {
            Anim.SetTrigger("Attack");
            await UniTask.Delay(_enemyBalancer.attackCooldown * 1000);

            if(_canAttack)
            {
                await BowAttack();
            }
        }
    }
}