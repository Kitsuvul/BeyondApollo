using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    private GameObject backgroundNebulaObj, backgroundStarsObj;
    private GameObject rocketShipObj, gameControllerObj, mainCameraObj, exitArrowObj, exitGateObj, soundObj, topLevelUIObj;
    private LevelLoader levelLoader;
    private CameraScript cameraScript;
    private HelperScript helperScript;

    private TopLevelUIHandler topLevelUIHandlerScript;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rocketShipObj = GameObject.FindGameObjectWithTag("Player");
        gameControllerObj = GameObject.FindGameObjectWithTag("GameController");
        mainCameraObj = GameObject.FindGameObjectWithTag("MainCamera");
        exitArrowObj = GameObject.FindGameObjectWithTag("ExitArrow");
        exitGateObj = GameObject.FindGameObjectWithTag("WinBox");
        soundObj = GameObject.FindGameObjectWithTag("SoundObject");
        backgroundNebulaObj = GameObject.FindGameObjectWithTag("Background");
        backgroundStarsObj = GameObject.FindGameObjectWithTag("BackgroundStars");

        levelLoader = gameControllerObj.GetComponent<LevelLoader>();
        cameraScript = mainCameraObj.GetComponent<CameraScript>();
        helperScript = this.gameObject.GetComponent<HelperScript>();

        topLevelUIObj = GameObject.FindGameObjectWithTag("CanvasUI");
        topLevelUIHandlerScript = topLevelUIObj.GetComponent<TopLevelUIHandler>();
        levelLoader = this.gameObject.GetComponent<LevelLoader>();
    }

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
        backgroundNebulaObj.GetComponent<BackgroundParallaxHandler>().ResetOnLevelLoad();
        backgroundStarsObj.GetComponent<BackgroundParallaxHandler>().ResetOnLevelLoad();
        cameraScript.ResetCamera();
        topLevelUIHandlerScript.ResetUIQueue();

        if (!helperScript.IsEndlessMode())
        {
            exitArrowObj.GetComponent<ExitArrowHandler>().ResetExitArrow();
            exitGateObj.GetComponent<Collider2D>().enabled = true;
        }
    }
}
