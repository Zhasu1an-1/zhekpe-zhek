using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("Result UI")]
    public GameObject resultPanel;
    public TMP_Text resultText;

    [Header("Audio")]
    public AudioSource ambientSound;
    public AudioSource randomArenaSounds;
    public AudioSource resultAudioSource;
    public AudioClip winMusic;
    public AudioClip loseMusic;

    [Header("Scene")]
    public string mainMenuSceneName = "MainMenu";
    public float returnDelay = 2f;

    private bool gameEnded = false;

    public void ShowWin()
    {
        ShowResult("YOU WIN", winMusic);
    }

    public void ShowLose()
    {
        ShowResult("YOU LOSE", loseMusic);
    }

    private void ShowResult(string message, AudioClip music)
    {
        if (gameEnded) return;
        gameEnded = true;

        Time.timeScale = 1f;

        if (ambientSound != null)
            ambientSound.Stop();

        if (randomArenaSounds != null)
            randomArenaSounds.Stop();

        if (resultPanel != null)
            resultPanel.SetActive(true);

        if (resultText != null)
        {
            resultText.text = message;
            StartCoroutine(AnimateResultText());
        }

        if (resultAudioSource != null && music != null)
        {
            resultAudioSource.Stop();
            resultAudioSource.clip = music;
            resultAudioSource.loop = false;
            resultAudioSource.Play();
        }

        StartCoroutine(ReturnToMainMenu());
    }

    private IEnumerator AnimateResultText()
    {
        Vector3 originalScale = resultText.transform.localScale;

        while (true)
        {
            resultText.transform.localScale = originalScale * 1.08f;
            yield return new WaitForSeconds(0.25f);

            resultText.transform.localScale = originalScale;
            yield return new WaitForSeconds(0.25f);
        }
    }

    private IEnumerator ReturnToMainMenu()
    {
        yield return new WaitForSeconds(returnDelay);

        SceneManager.LoadScene(mainMenuSceneName);
    }
}