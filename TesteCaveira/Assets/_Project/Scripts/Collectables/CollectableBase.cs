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
        protected PlayerController Player;
        protected IAudioPlayer AudioPlayer;

        public void SetupCollectable(GameManager manager, PlayerController player)
        {
            Collider.enabled = true;
            Player = player;
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