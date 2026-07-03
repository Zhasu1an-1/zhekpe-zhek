using UnityEngine;

public class AmbientSoundStopper : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void StopAmbient()
    {
        audioSource.Stop();
    }
}