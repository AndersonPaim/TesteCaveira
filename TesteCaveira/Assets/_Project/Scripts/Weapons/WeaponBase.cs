using UnityEngine;
using Interfaces;

namespace Weapons
{
    public abstract class WeaponBase : MonoBehaviour
    {
        [SerializeField] protected float Damage;

        protected void DoDamage(GameObject obj)
        {
            IDamageable damageable = obj.GetComponent<IDamageable>();
            DamageFlash flash = obj.GetComponent<DamageFlash>();
            damageable.TakeDamage(Damage);
            flash?.Flash();
        }

        protected bool CanDoDamage(GameObject obj)
        {
            IDamageable damageable = obj.GetComponent<IDamageable>();

            if(damageable != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}