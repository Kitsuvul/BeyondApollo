/*
Notes:

*/
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TopLevelUIHandler : MonoBehaviour
{
    [SerializeField] protected GameObject levelPanelObj, inGamePanelObj, settingsPanelObj, winStatePanelObj, failStatePanelObj, loadingScreenPanelObj, endlessPanelObj, endlessTutorialPanelObj;

    protected GameObject rocketShipObj, gameControllerObj, mainCameraObj, exitArrowObj, exitGateObj, soundObj, settingsObj;
    protected LevelLoader levelLoader;
    protected CameraScript cameraScript;
    protected HelperScript helperScript;
    protected ExitArrowHandler exitArrowHandler;

    private bool uiIsOpen = false;
    private bool uiHasQueue = false;
    private BitArray uiToOpen;

    public bool UIIsOpen
    {
        get { return uiIsOpen; }
        private set { uiIsOpen = value; }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        // Getting Objects
        rocketShipObj = GameObject.FindGameObjectWithTag("Player");
        gameControllerObj = GameObject.FindGameObjectWithTag("GameController");
        mainCameraObj = GameObject.FindGameObjectWithTag("MainCamera");
        exitArrowObj = GameObject.FindGameObjectWithTag("ExitArrow");
        exitGateObj = GameObject.FindGameObjectWithTag("WinBox");
        soundObj = GameObject.FindGameObjectWithTag("SoundObject");
        settingsObj = GameObject.FindGameObjectWithTag("PersistSettings");

        levelLoader = gameControllerObj.GetComponent<LevelLoader>();
        cameraScript = mainCameraObj.GetComponent<CameraScript>();
        helperScript = gameControllerObj.GetComponent<HelperScript>();

        uiToOpen = new BitArray(5);


        if (helperScript.IsStoryMode())
        {
            exitArrowHandler = exitArrowObj.GetComponent<ExitArrowHandler>();
        }

        if(helperScript.IsEndlessMode())
        {
            if (!settingsObj.GetComponent<SaveManager>().GetIsEndlessTutorialCompleteInPlayerPrefs())
            {
                OpenEndlessTutorialPanel();
            }
        }

        // On first load
        OpenLevelPanel();
    }

    void Update()
    {
        CheckIfUIPanelsAreOpen();
        QueueNextUIPanel();
    }

    #region Methods
    public void ResetToLoadNextLevel()
    {
        BaseReset();
        levelLoader.LoadNextLevel();
    }

    public void ResetToLoadSpecificLevel(int level)
    {
        BaseReset();
        levelLoader.LoadSpecificLevel(level);
    }

    public void ResetLevel()
    {
        if (rocketShipObj == null)
        {
            Instantiate(Resources.Load("SpaceShip"));
        }

        BaseReset();

        soundObj.GetComponent<SoundHandler>().StopAudioRocketFlyingClip();
        levelLoader.ReloadLevel();
    }

    private void BaseReset()
    {
        rocketShipObj.GetComponent<ShipControlsScript>().ResetShipToStart();
        gameControllerObj.GetComponent<DeathHandler>().ResetDebris();
        rocketShipObj.GetComponent<PreviousPathHandler>().OnReset();
        cameraScript.ResetCamera();
        ResetUIQueue();

        if (!helperScript.IsEndlessMode())
        {
            exitArrowHandler.ResetExitArrow();
            exitGateObj.GetComponent<Collider2D>().enabled = true;
        }
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void QueueNextUIPanel()
    {
        if(uiIsOpen || !uiHasQueue)
        {
            return;
        }

        if (uiToOpen[0])
        {
            OpenSettingsPanel();
            uiToOpen[0] = false;
        }

        if (uiToOpen[1])
        {
            OpenWinStatePanel();
            uiToOpen[1] = false;
        }

        if (uiToOpen[2])
        {
            OpenLevelPanel();
            uiToOpen[2] = false;
        }

        if (uiToOpen[3])
        {
            OpenFailStatePanel();
            uiToOpen[3] = false;
        }

        if(uiToOpen[4])
        {
            OpenEndlessPanel();
            uiToOpen[4] = false;
        }

        uiHasQueue = false;

        foreach (bool panel in uiToOpen)
        {
            if(panel)
            {
                uiHasQueue = true;
                return;
            }
        }
    }

    public void ResetUIQueue()
    {
        uiHasQueue = false;
        uiToOpen.SetAll(false);
    }

    #endregion

    #region Open/Close Panel Methods

    public void OpenInGamePanel()
    {
        if (inGamePanelObj != null)
        {
            inGamePanelObj.SetActive(true);
            uiIsOpen = false;
        }
    }

    public void CloseInGamePanel()
    {
        if (inGamePanelObj != null)
        {
            inGamePanelObj.SetActive(false);
        }
    }

    public void OpenSettingsPanel()
    {
        if (settingsPanelObj != null)
        {
            if(uiIsOpen && !settingsPanelObj.activeSelf)
            {
                uiToOpen[0] = true;
                uiHasQueue = true;
                return;
            }
            settingsPanelObj.SetActive(true);
        }
    }

    public void CloseSettingsPanel()
    {
        if (settingsPanelObj != null)
        {
            settingsPanelObj.SetActive(false);
        }
    }

    public void OpenWinStatePanel()
    {
        if (winStatePanelObj != null)
        {
            if (uiIsOpen && !winStatePanelObj.activeSelf)
            {
                uiToOpen[1] = true;
                uiHasQueue = true;
                return;
            }
            winStatePanelObj.SetActive(true);
        }
    }
    
    public void CloseWinStatePanel()
    {
        if (winStatePanelObj != null)
        {
            winStatePanelObj.SetActive(false);
        }
    }

    public void OpenEndlessPanel()
    {
        if (endlessPanelObj != null)
        {
            if (uiIsOpen && !endlessPanelObj.activeSelf)
            {
                uiToOpen[4] = true;
                uiHasQueue = true;
                return;
            }

            int highscore = settingsObj.GetComponent<SaveManager>().GetEndlessHighScoreInPlayerPrefs();
            Debug.Log(highscore);
            int currentRun = rocketShipObj.GetComponent<BaseShipScript>().EndlessScore;
            if (highscore > currentRun)
            {
                endlessPanelObj.GetComponentInChildren<Text>().text = "Highscore:\n" + highscore + "\nScore: " + currentRun;
            }
            else
            {
                settingsObj.GetComponent<SaveManager>().SetEndlessHighScoreInPlayerPrefs(currentRun);
                endlessPanelObj.GetComponentInChildren<Text>().text = "New Highscore:\n" + currentRun;
            }
            
            endlessPanelObj.SetActive(true);
        }
    }

    public void CloseEndlessPanel()
    {
        if (endlessPanelObj != null)
        {
            endlessPanelObj.SetActive(false);
        } 
    }

    public void OpenEndlessTutorialPanel()
    {
        if (winStatePanelObj != null)
        {
            winStatePanelObj.SetActive(true);
        }
    }

    public void CloseEndlessTutorialPanel()
    {
        if (endlessPanelObj != null)
        {
            endlessPanelObj.SetActive(false);
        }
    }

    public void OpenLevelPanel()
    {
        if (levelPanelObj != null)
        {
            if (uiIsOpen && !levelPanelObj.activeSelf)
            {
                uiToOpen[2] = true;
                uiHasQueue = true;
                return;
            }
            levelPanelObj.SetActive(true);
            levelPanelObj.GetComponent<LevelUIPanelHandler>().loadedAllButtons = false;
        }
    }

    public void CloseLevelPanel()
    {
        if (levelPanelObj != null)
        {
            levelPanelObj.SetActive(false);
        }
    }

    public void OpenFailStatePanel()
    {
        if (failStatePanelObj != null)
        {
            if (uiIsOpen && !failStatePanelObj.activeSelf)
            {
                Debug.Log("???????????????????????");
                uiToOpen[3] = true;
                uiHasQueue = true;
                return;
            }

            failStatePanelObj.SetActive(true);
        }
    }

    public void CloseFailStatePanel()
    {
        if (failStatePanelObj != null)
        {
            failStatePanelObj.SetActive(false);
        }
    }

    private void CheckIfUIPanelsAreOpen()
    {
        if(helperScript.IsEndlessMode())
        {
            if(endlessPanelObj.activeSelf == true)
            {
                uiIsOpen = true;
            }
            else
            {
                uiIsOpen = false;
            }
        }
        else
        {
            if (failStatePanelObj.activeSelf == true || levelPanelObj.activeSelf == true || winStatePanelObj.activeSelf == true || settingsPanelObj.activeSelf == true)
            {
                uiIsOpen = true;
            }
            else
            {
                uiIsOpen = false;
            }
        }
    }
    #endregion
}
