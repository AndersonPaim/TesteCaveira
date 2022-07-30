using UnityEngine;

namespace _Project.Scripts
{
    public struct InputData
    {
        public Vector2 Movement;
        public float LookX;
        public float LookY;

        public bool IsJumping;
        public bool IsAiming;
        public bool IsShooting;
    }
}