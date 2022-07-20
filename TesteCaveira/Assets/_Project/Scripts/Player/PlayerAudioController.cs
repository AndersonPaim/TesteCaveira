using UnityEngine;
using Managers;
using Interfaces;

public class PlayerAudioController : MonoBehaviour
{
    [SerializeField] private AudioManager _audioManager;
    [SerializeField] private SoundEffect _shootAudio;
    [SerializeField] private SoundEffect _damageAudio;
    [SerializeField] private SoundEffect _bowAudio;

    private IAudioPlayer _audioPlayer;


    public void PlayShootAudio()
    {
        _audioPlayer.PlayAudio(_shootAudio, transform.position);
    }

    public void PlayDamageAudio()
    {
        _audioPlayer.PlayAudio(_damageAudio, transform.position);
    }

    public void PlayBowAudio()
    {
        _audioPlayer.PlayAudio(_bowAudio, transform.position);
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        _audioPlayer = _audioManager.GetComponent<IAudioPlayer>();
    }
}
