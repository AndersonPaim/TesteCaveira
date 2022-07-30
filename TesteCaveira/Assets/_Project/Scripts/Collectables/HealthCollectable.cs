using _Project.Scripts.Interfaces;
using UnityEngine;

namespace _Project.Scripts.Collectables
{
    public class HealthCollectable : CollectableBase
    {
        [SerializeField] private float _healAmount;

        private void OnTriggerEnter(Collider other)
        {
            CollectItem(other.gameObject);
        }

        protected override void CollectItem(GameObject obj)
        {
            base.CollectItem(obj);

            IHealable healable = obj.GetComponent<IHealable>();

            if(healable != null)
            {
                healable.Heal(_healAmount);
            }
        }
    }
}