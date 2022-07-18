using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerUps;

namespace Enemy
{
    [CreateAssetMenu(fileName = "New EnemyBalancer", menuName = "EnemyBalancer")]
    public class EnemyBalancer : ScriptableObject
    {
        public float health;
        public float attackDistance;
        public float viewAngle;
        public float shootForce;
        public int stunCooldown;
        public int attackCooldown;
        public int destroyDelay;
        public List<PowerUp> drops;
    }
}