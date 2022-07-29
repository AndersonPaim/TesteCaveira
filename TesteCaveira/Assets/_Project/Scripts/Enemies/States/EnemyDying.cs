using UnityEngine;
using UnityEngine.AI;
using Cysharp.Threading.Tasks;
using Managers;
using Collectable.PowerUp;
using Collectable;

namespace Enemy
{
    public class EnemyDying : StateMachine
    {
        private int _destroyDelay;

        public EnemyDying(GameObject enemy, GameObject player, NavMeshAgent agent, SkinnedMeshRenderer mesh, Animator anim, NavMeshPath path, EnemyBalancer balancer, GameManager manager)
                    : base(enemy, player, agent, mesh, anim, path, balancer, manager)
        {
            CurrentState = States.DYING;
            _destroyDelay = balancer.destroyDelay;
        }

        protected override void Enter()
        {
            Agent.SetDestination(Enemy.transform.position);
            Anim.SetTrigger("Death");
            DropPowerUp();
            DisableObjectASync();
            base.Enter();
        }

        private async UniTask DisableObjectASync()
        {
            await UniTask.Delay(_destroyDelay * 1000);
            Enemy.gameObject.SetActive(false);
        }

        private void DropPowerUp()
        {
            foreach(PowerUp powerUp in Balancer.drops)
            {
                int randomValue = Random.Range(1, 101);

                if(powerUp.DropRate > randomValue)
                {
                    InstantiatePowerUp(powerUp.prefab);
                    break;
                }
            }
        }

        private void InstantiatePowerUp(GameObject prefab)
        {
            GameObject obj = Manager.ObjectPooler.SpawnFromPool(prefab.GetInstanceID());
            obj.transform.position = Enemy.transform.position;
            CollectableBase collectable = obj.GetComponent<CollectableBase>();
            collectable.SetupCollectable(Manager);
        }
    }
}