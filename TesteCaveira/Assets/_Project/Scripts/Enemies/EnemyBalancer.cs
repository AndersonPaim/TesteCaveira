using System.Collections.Generic;
using _Project.Scripts.Collectables;
using UnityEngine;

namespace _Project.Scripts.Enemies
{
    [CreateAssetMenu(fileName = "New EnemyBalancer", menuName = "EnemyBalancer")]
    public class EnemyBalancer : ScriptableObject
    {
        public float health;
        public float attackDistance;
        public float shootForce;
        public float walkSpeed;
        public float walkTime;
        public float blockDuration;
        public float blockSpeedMultiplier;
        public int stunCooldown;
        public int attackCooldown;
        public int destroyDelay;
        public int killScore;
        public List<PowerUp> drops;
        public List<Material> textures;
    }
}