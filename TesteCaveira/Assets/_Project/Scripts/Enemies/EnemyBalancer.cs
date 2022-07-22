using System.Collections.Generic;
using UnityEngine;
using Collectable.PowerUp;

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
        public int killScore;
        public List<PowerUp> drops;
    }
}