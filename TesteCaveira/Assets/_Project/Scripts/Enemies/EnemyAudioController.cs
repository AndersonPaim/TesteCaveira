using UnityEngine;
using Managers;
using Interfaces;

namespace Enemy
{
    public class EnemyAudioController : MonoBehaviour
    {
        [SerializeField] private SoundEffect _footstepAudio;
        [SerializeField] private SoundEffect _damageAudio;
        [SerializeField] private SoundEffect _attackAudio;

        private AudioManager _audioManager;
        private IAudioPlayer _audioPlayer;

        public void SetupManager(AudioManager audioManager)
        {
            _audioManager = audioManager;
            _audioPlayer = _audioManager.GetComponent<IAudioPlayer>();
        }

        public void PlayFootspeedAudio()
        {
            _audioPlayer.PlayAudio(_footstepAudio, transform.position);
        }

        public void PlayDamageAudio()
        {
            _audioPlayer.PlayAudio(_damageAudio, transform.position);
        }

        public void PlayAttackAudio()
        {
            _audioPlayer.PlayAudio(_attackAudio, transform.position);
        }
    }
}