using UnityEngine;
using System.Collections;

public class RandomArenaSounds : MonoBehaviour
{
    public AudioSource source;
    public AudioClip[] clips;

    public float minDelay = 10f;
    public float maxDelay = 25f;

    private void Start()
    {
        StartCoroutine(PlayRandomSounds());
    }

    IEnumerator PlayRandomSounds()
    {
        while (true)
        {
            float waitTime = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(waitTime);

            if (clips.Length > 0)
            {
                AudioClip clip = clips[Random.Range(0, clips.Length)];
                source.PlayOneShot(clip);
            }
        }
    }
}