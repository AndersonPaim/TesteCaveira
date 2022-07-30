using _Project.Scripts.Collectables;
using _Project.Scripts.Managers;
using _Project.Scripts.Player;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace _Project.Scripts.Enemies.States
{
    public class EnemyDying : StateMachine
    {
        private int _destroyDelay;

        public EnemyDying(GameObject enemy, GameObject player, NavMeshAgent agent, SkinnedMeshRenderer mesh, Animator anim, EnemyBalancer balancer, WaypointController waypoints)
                    : base(enemy, player, agent, mesh, anim, balancer, waypoints)
        {
            CurrentState = EnemyStates.DYING;
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
            GameObject obj = ObjectPooler.sInstance.SpawnFromPool(prefab.GetInstanceID());
            obj.transform.position = Enemy.transform.position;
            CollectableBase collectable = obj.GetComponent<CollectableBase>();
            PlayerController player = Player.GetComponent<PlayerController>();
            collectable.SetupCollectable(player);
        }
    }
}