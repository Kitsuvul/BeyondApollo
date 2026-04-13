/*
Notes:

*/
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private GameObject titlePanel, settingsPanel, creditsPanel, tutorialWarningPanel;
    [SerializeField] private GameObject tutorialButton, challengeButton, endlessButton;
    private GameObject settingsObj;

    private bool tutorialComplete = false;

    private void Awake()
    {
        settingsObj = GameObject.FindGameObjectWithTag("PersistSettings");

        if(settingsObj != null )
        {
            tutorialComplete = settingsObj.GetComponent<SaveManager>().GetIsTutorialCompleteInPlayerPrefs();
        }

        if (tutorialComplete)
        {
            challengeButton.SetActive(true);
            endlessButton.SetActive(true);
            tutorialButton.SetActive(false);
        }
        else
        {
            endlessButton.SetActive(false);
            challengeButton.SetActive(false);
            tutorialButton.SetActive(true);
        }
    }

    public void StartChallenge()
    {
        SceneManager.LoadScene(1);
    }

    public void StartTutorial()
    {
        SceneManager.LoadScene(2);
    }
    public void StartEndless()
    {
        SceneManager.LoadScene(3);
    }

    public void TitleMenuShow()
    {
        titlePanel.SetActive(true);
    }

    public void TitleMenuHide()
    {
        titlePanel.SetActive(false);
    }

    public void SettingsMenuShow()
    {
        settingsPanel.SetActive(true);
    }

    public void SettingsMenuHide()
    {
        settingsPanel.SetActive(false);
    }

    public void TutorialWarningPanelShow()
    {
        tutorialWarningPanel.SetActive(true);
    }

    public void TutorialWarningPanelHide()
    {
        tutorialWarningPanel.SetActive(false);
    }

    public void CreditsShow()
    {
        creditsPanel.SetActive(true);
    }

    public void CreditsHide()
    {
        creditsPanel.SetActive(false);
    }

    public void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    public void ExitGame()
    {
        PlayerPrefs.Save();
        Application.Quit();
    }
}
