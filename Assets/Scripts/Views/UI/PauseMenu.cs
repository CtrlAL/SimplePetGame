using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] public Button StartRestartButton;
    [SerializeField] public Button ResumeButton;
    [SerializeField] public Button ExitButton;

    [SerializeField] private TMP_Text StartRestartButtonText;

    public void SetStartedMenu()
    {
        ResumeButton.gameObject.SetActive(true);
        StartRestartButtonText.text = "Restart";
    }

    public void SetNotStartedMenu()
    {
        ResumeButton.gameObject.SetActive(false);
        StartRestartButtonText.text = "Start";
    }
}