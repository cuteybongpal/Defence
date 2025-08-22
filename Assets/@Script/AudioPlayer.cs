using System.Collections;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public bool IsPlaying;

    AudioSource audioSource;
    public void Play(AudioClip audioClip)
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(WaitAudioPlay());
    }

    IEnumerator WaitAudioPlay()
    {
        audioSource.Play();
        IsPlaying = true;
        while (audioSource.isPlaying)
            yield return null;

        gameObject.SetActive(false);
        IsPlaying = false;
    }
}
