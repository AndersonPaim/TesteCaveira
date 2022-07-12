using UnityEngine;
using Interfaces;

public class HitCollider : MonoBehaviour
{
    [SerializeField] private float _damage;

    private bool _canDamage = true;

    protected virtual void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
        DamageFlash flash = other.gameObject.GetComponent<DamageFlash>();

        if(damageable != null)
        {
            damageable.TakeDamage(_damage);
        }

        if(flash != null)
        {
            flash.Flash();
        }
    }
}