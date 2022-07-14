using UnityEngine;
using Interfaces;

public class DefaultSword : WeaponBase
{
    [SerializeField] private float _damage;

    protected virtual void OnTriggerEnter(Collider other)
    {
        if(CanDoDamage(other.gameObject))
        {
            DoDamage(other.gameObject);
        }
    }
}