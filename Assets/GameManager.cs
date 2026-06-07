using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject resultTextObject;
    public TMP_Text resultText;

    public void ShowWin()
    {
        if (resultTextObject != null)
            resultTextObject.SetActive(true);

        if (resultText != null)
            resultText.text = "YOU WIN";

        Time.timeScale = 0f;
    }

    public void ShowLose()
    {
        if (resultTextObject != null)
            resultTextObject.SetActive(true);

        if (resultText != null)
            resultText.text = "YOU LOSE";

        Time.timeScale = 0f;
    }
}