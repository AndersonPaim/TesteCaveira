
using UnityEngine;

namespace Collectable.PowerUp
{
    public class DamageBuffCollectable : CollectableBase
    {
        [SerializeField] private float _damageMultiplier;
        [SerializeField] private int _buffDuration;

        private void OnTriggerEnter(Collider other)
        {
            CollectItem(other.gameObject);
        }

        protected override void CollectItem(GameObject obj)
        {
            base.CollectItem(obj);
            //TODO REMEDIAR O BUFF COM O PLAYER
            Manager.BowController.SetDamageBuff(_damageMultiplier, _buffDuration);
        }
    }
}