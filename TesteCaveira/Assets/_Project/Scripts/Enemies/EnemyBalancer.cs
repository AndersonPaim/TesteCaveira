using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    [CreateAssetMenu(fileName = "New EnemyBalancer", menuName = "EnemyBalancer")]
    public class EnemyBalancer : ScriptableObject
    {
        public float attackDistance;
        public float viewAngle;
        public float shootForce;
        public int attackCooldown;
    }
}