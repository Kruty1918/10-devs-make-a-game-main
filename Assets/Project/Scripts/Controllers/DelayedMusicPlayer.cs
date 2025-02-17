using System.Collections;
using UnityEngine;

public class DelayedMusicPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip musicClip;
    public float delayBeforeReplay = 600f; 

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        audioSource.clip = musicClip;
        audioSource.Play();
        StartCoroutine(WaitForMusicToEnd());
    }

    IEnumerator WaitForMusicToEnd()
    {
        while (true)
        {
            yield return new WaitUntil(() => !audioSource.isPlaying);
            yield return new WaitForSeconds(delayBeforeReplay);
            audioSource.Play();
        }
    }
}
