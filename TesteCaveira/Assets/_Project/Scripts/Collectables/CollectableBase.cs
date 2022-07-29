using Coimbra.Services;
using Coimbra.Services.Events;
using Interfaces;
using Managers;
using UnityEngine;

namespace Collectable
{
    public class CollectableBase : MonoBehaviour
    {
        [SerializeField] private SoundEffect _collectAudio;

        protected Collider Collider;
        protected PlayerController Player;
        protected IAudioPlayer AudioPlayer;
        protected IEventService EventService;

        public void SetupCollectable(PlayerController player)
        {
            Collider.enabled = true;
            Player = player;
            AudioPlayer = AudioManager.sInstance.GetComponent<IAudioPlayer>();
        }

        private void Awake()
        {
            Initialize();
        }

        protected virtual void Initialize()
        {
            Collider = GetComponent<Collider>();
            EventService = ServiceLocator.Get<IEventService>();
        }

        protected virtual void CollectItem(GameObject obj)
        {
            AudioPlayer.PlayAudio(_collectAudio, transform.position);
            Collider.enabled = false;
            gameObject.SetActive(false);
        }
    }
}