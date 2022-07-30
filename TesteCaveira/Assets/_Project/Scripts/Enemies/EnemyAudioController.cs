using _Project.Scripts.Interfaces;
using _Project.Scripts.Managers;
using UnityEngine;

namespace _Project.Scripts.Enemies
{
    public class EnemyAudioController : MonoBehaviour
    {
        [SerializeField] private SoundEffect _footstepAudio;
        [SerializeField] private SoundEffect _damageAudio;
        [SerializeField] private SoundEffect _attackAudio;
        [SerializeField] private SoundEffect _shieldAttackAudio;
        [SerializeField] private SoundEffect _shieldBlockAudio;

        private AudioManager _audioManager;
        private IAudioPlayer _audioPlayer;

        public void SetupManager()
        {
            _audioPlayer = AudioManager.Instance.GetComponent<IAudioPlayer>();
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
        
        public void PlayShieldAttackAudio()
        {
            _audioPlayer.PlayAudio(_shieldAttackAudio, transform.position);
        }
        
        public void PlayShieldBlockAudio()
        {
            _audioPlayer.PlayAudio(_shieldBlockAudio, transform.position);
        }
    }
}