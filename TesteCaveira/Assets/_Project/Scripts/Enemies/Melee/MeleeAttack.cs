using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy.Melee
{
    public class MeleeAttack : StateMachine
    {
        private Vector3 _rayPosition;
        private EnemyBalancer _enemyBalancer;
        private bool _canAttack;

        public MeleeAttack(GameObject enemy, GameObject player, NavMeshAgent agent, Animator anim, NavMeshPath path, EnemyBalancer enemyBalancer)
                    : base(enemy, player, agent, anim, path)
        {
            currentState = States.ATTACK;
            _enemyBalancer = enemyBalancer;
        }

        public override void Enter()
        {
            base.Enter();
            Attack();
            _canAttack = true;
        }

        public override void Update()
        {
            base.Update();
            Debug.Log("ATTACKING");
            SearchPlayer();
        }

        private void SearchPlayer()
        {
            float targetDistance = Vector3.Distance(Enemy.transform.position, Player.transform.position);

            Enemy.transform.LookAt(Player.transform);

            if(targetDistance > _enemyBalancer.attackDistance)
            {
                _canAttack = false;
                LostPlayer();
            }
        }

        private void LostPlayer()
        {
            NextState = new MeleeMoving(Enemy, Player, Agent, Anim, Path, _enemyBalancer);
            Stage = Events.EXIT;
        }

        private async UniTask Attack()
        {
            Anim.SetTrigger("Attack");
            await UniTask.Delay(_enemyBalancer.attackCooldown * 1000);

            if(_canAttack)
            {
                await Attack();
            }
        }
    }
}