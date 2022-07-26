using UnityEngine;
using UnityEngine.AI;
using Interfaces;
using Managers;

namespace Enemy.Archer
{
    public class ArcherAI : EnemyBase, IDamageable
    {
        [SerializeField] private Transform _shootPosition;
        [SerializeField] private GameObject _arrowPrefab;

        public void Attack()
        {
            GameObject arrow = Manager.ObjectPooler.SpawnFromPool(_arrowPrefab.GetInstanceID());
            arrow.transform.position =  _shootPosition.position;
            Rigidbody rb = arrow.GetComponent<Rigidbody>();
            rb.velocity = _shootPosition.transform.forward * EnemyBalancer.shootForce;
        }
    }
}

