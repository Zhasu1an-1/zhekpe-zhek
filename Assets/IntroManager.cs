using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    public float introDuration = 6f;

    void Start()
    {
        Invoke(nameof(LoadMainMenu), introDuration);
    }

    void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}