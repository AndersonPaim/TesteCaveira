
using Managers;
using UnityEngine;

namespace Collectable
{
    public class CollectableBase : MonoBehaviour
    {
        protected Collider Collider;
        protected GameManager Manager;

        public void SetupCollectable(GameManager manager)
        {
            Collider.enabled = true;
            Manager = manager;
        }

        private void Start()
        {
            Initialize();
        }

        protected virtual void Initialize()
        {
            Collider = GetComponent<Collider>();
        }

        protected virtual void CollectItem(GameObject obj)
        {
            Collider.enabled = false;
            gameObject.SetActive(false);
        }
    }
}