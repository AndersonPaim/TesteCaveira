using UnityEngine;

namespace _Project.Scripts.Player
{
    [CreateAssetMenu(fileName = "New PlayerBalancer", menuName = "PlayerBalancer")]
    public class PlayerBalancer : ScriptableObject
    {
        public float health;
        public float speed;
        public float acceleration;
        public float deceleration;
        public float jumpForce;

    }
}