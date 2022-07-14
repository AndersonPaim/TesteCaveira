using UnityEngine;
using Interfaces;

public abstract class WeaponBase : MonoBehaviour
{
    [SerializeField] protected float _damage;

    protected void DoDamage(GameObject obj)
    {
        IDamageable damageable = obj.GetComponent<IDamageable>();
        DamageFlash flash = obj.GetComponent<DamageFlash>();
        damageable.TakeDamage(_damage);
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