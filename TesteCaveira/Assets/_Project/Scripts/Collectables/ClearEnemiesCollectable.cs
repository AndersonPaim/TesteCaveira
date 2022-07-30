using _Project.Scripts.Events;
using UnityEngine;

namespace _Project.Scripts.Collectables
{
    public class ClearEnemiesCollectable : CollectableBase
    {
        private void OnTriggerEnter(Collider other)
        {
            CollectItem(other.gameObject);
        }

        protected override void CollectItem(GameObject obj)
        {
            base.CollectItem(obj);
            OnClearEnemies clearEnemies = new OnClearEnemies();
            clearEnemies?.Invoke(EventService);
        }
    }
}