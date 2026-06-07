using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuUI : MonoBehaviour
{
    public AudioSource clickAudio;
    public float delayBeforeStart = 1f;
    
    public int fightSceneIndex = 3;

    public void StartDuel()
    {
        StartCoroutine(StartDuelRoutine());
    }

    IEnumerator StartDuelRoutine()
    {
        if (clickAudio != null)
        {
            clickAudio.Stop();
            clickAudio.Play();
        }

        yield return new WaitForSeconds(delayBeforeStart);

        SceneManager.LoadScene(fightSceneIndex);
    }
}