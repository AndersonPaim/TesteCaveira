using _Project.Scripts.Interfaces;
using UnityEngine;

namespace _Project.Scripts.Weapons
{
    public abstract class WeaponBase : MonoBehaviour
    {
        [SerializeField] protected float Damage;
        private float _damageMultiplier = 1;

        public void SetDamageMultiplier(float multiplier)
        {
            _damageMultiplier = multiplier;
        }

        protected void DoDamage(GameObject obj)
        {
            IDamageable damageable = obj.GetComponent<IDamageable>();
            DamageFlash flash = obj.GetComponent<DamageFlash>();
            damageable.TakeDamage(Damage * _damageMultiplier);
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