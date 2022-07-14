using UnityEngine;

public class DefaultSword : WeaponBase
{
    protected virtual void OnTriggerEnter(Collider other)
    {
        if(CanDoDamage(other.gameObject))
        {
            DoDamage(other.gameObject);
        }
    }
}