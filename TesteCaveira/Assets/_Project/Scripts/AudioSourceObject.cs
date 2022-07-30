using System.Collections;
using UnityEngine;

namespace _Project.Scripts
{
    public class AudioSourceObject : MonoBehaviour
    {
        private AudioClip _audioClip;
        private void OnEnable()
        {
            _audioClip = GetComponent<AudioSource>().clip;

            if(_audioClip != null)
            {
                StartCoroutine(DisableObject(_audioClip.length));
            }
        }

        private IEnumerator DisableObject(float time)
        {
            yield return new WaitForSeconds(time);
            gameObject.SetActive(false);
        }
    }
}