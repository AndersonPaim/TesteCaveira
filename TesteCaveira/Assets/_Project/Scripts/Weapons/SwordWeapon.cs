using UnityEngine;

namespace Weapons
{
    public class SwordWeapon : WeaponBase
    {
        protected virtual void OnTriggerEnter(Collider other)
        {
            if(CanDoDamage(other.gameObject))
            {
                DoDamage(other.gameObject);
            }
        }
    }
}