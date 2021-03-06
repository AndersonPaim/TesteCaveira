using Interfaces;
using Managers;
using UnityEngine;

namespace Collectable
{
    public class CollectableBase : MonoBehaviour
    {
        [SerializeField] private SoundEffect _collectAudio;

        protected Collider Collider;
        protected GameManager Manager;
        protected IAudioPlayer AudioPlayer;

        public void SetupCollectable(GameManager manager)
        {
            Collider.enabled = true;
            Manager = manager;
            AudioPlayer = Manager.AudioManager.GetComponent<IAudioPlayer>();
        }

        private void Awake()
        {
            Initialize();
        }

        protected virtual void Initialize()
        {
            Collider = GetComponent<Collider>();
        }

        protected virtual void CollectItem(GameObject obj)
        {
            AudioPlayer.PlayAudio(_collectAudio, transform.position);
            Collider.enabled = false;
            gameObject.SetActive(false);
        }
    }
}