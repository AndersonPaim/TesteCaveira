using UnityEngine;

namespace _Project.Scripts.Weapons
{
    public class SwordWeapon : WeaponBase
    {
        private void OnTriggerEnter(Collider other)
        {
            if(CanDoDamage(other.gameObject))
            {
                DoDamage(other.gameObject);
            }
        }
    }
}