/*
Notes:


*/
using System.Collections;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    #region Private Variables
    private float OrthoZoomSpeed = 0.1f;
    private bool rotateCam = false;
    private bool hasAnimated = false;
    private bool hasAnimatedLevelIntro = false;
    private Vector3 cameraStartPos = new Vector3(0.0f, 47.5f, -10.0f);
    private GameObject rocketShipObj, gameControllerObj, canvasObj;
    private ShipControlsScript rocketControllerScript;
    private LevelLoader levelLoaderScript;
    private InputScript inputScript;
    private HelperScript helperScript;
    private Animation cameraAnim;
    private TopLevelUIHandler uiScript;
    private TutorialHandler tutorialScript;

    // New Variables for behind Arrow
    private Vector3 cameraStartPosPostAnimBehindArrow = new Vector3(0.0f, 10f, -10.0f);
    private float cameraOrthoStartViewBehindArrow = 20f;


    #endregion

    #region Properties
    public bool HasAnimated
    {
        get { return hasAnimated; }
        private set { hasAnimated = value; }
    }

    public bool HasAnimatedLevelIntro
    {
        get { return hasAnimatedLevelIntro; }
        private set { hasAnimatedLevelIntro = value; }
    }
    #endregion

    #region Unity Functions
    void Awake()
    {
        canvasObj = GameObject.FindGameObjectWithTag("CanvasUI");
        rocketShipObj = GameObject.FindGameObjectWithTag("Player");
        gameControllerObj = GameObject.FindGameObjectWithTag("GameController");

        inputScript = gameControllerObj.GetComponent<InputScript>();
        helperScript = gameControllerObj.GetComponent<HelperScript>();
        levelLoaderScript = gameControllerObj.GetComponent<LevelLoader>();
        uiScript = canvasObj.GetComponent<TopLevelUIHandler>();
        tutorialScript = canvasObj.GetComponent<TutorialHandler>();
        rocketControllerScript = rocketShipObj.GetComponent<ShipControlsScript>();

        cameraAnim = this.GetComponent<Animation>();
    }

    void Update()
    {
        if (tutorialScript || !uiScript.UIIsOpen)
        {
            UpdateZoom();

            if (rocketControllerScript.HasLaunched && !hasAnimated)
            {
                UpdateCameraPositionOnLaunch();
            }

            if (levelLoaderScript && levelLoaderScript.LevelLoaded && !hasAnimatedLevelIntro)
            {
                PlayOnLevelLoadAnimation();
            }
        }

        if(rotateCam)
        {
            StartCoroutine(RotateCamera());
        }
    }
    #endregion

    #region Methods
    /// <summary>
    /// Resets the camera to it's original starting point
    /// </summary>
    public void ResetCamera()
    {
        cameraAnim.Stop();
        this.gameObject.transform.parent = null;
        this.gameObject.transform.position = cameraStartPos;
        this.gameObject.transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
        hasAnimated = false;
        hasAnimatedLevelIntro = false;
    }

    /// <summary>
    /// Plays the intro camera animation on loading or reseting a level
    /// </summary>
    public void PlayOnLevelLoadAnimation()
    {
        cameraAnim["IntroShotBehindArrow"].wrapMode = WrapMode.Once;
        cameraAnim.Play("IntroShotBehindArrow");
        hasAnimatedLevelIntro = true;
        Debug.Log("Played");
    }

    /// <summary>
    /// Plays a animation for the camera to rotate and lock with the rocket ship
    /// </summary>
    private void UpdateCameraPositionOnLaunch()
    {
        float magnitude = (helperScript.GetVec2FromPositionHelper(rocketShipObj.transform.position) - rocketControllerScript.StartPosition).magnitude;
        if (magnitude >= 5.5f && hasAnimated == false)
        {
            this.transform.parent = rocketShipObj.transform;
            rotateCam = true;
            this.transform.localPosition = new Vector3(10000.0f, 100000.0f, -14.6f);
            cameraAnim["CameraZoomOut"].wrapMode = WrapMode.Once;
            cameraAnim.Play("CameraZoomOut");
            hasAnimated = true;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private IEnumerator RotateCamera()
    {
        if(this.transform.rotation == rocketShipObj.transform.rotation)
        {
            rotateCam = false;
            yield return null;
        }

        while (this.transform.rotation != rocketShipObj.transform.rotation)
        {
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rocketShipObj.transform.rotation, Time.deltaTime/2);
            yield return null;
        }
        yield return null;
    }

    /// <summary>
    /// Standard update for handling the user inputed zoom controls
    /// </summary>
    private void UpdateZoom()
    {
        if (inputScript.CheckDoubleTouch())
        {
            // Gets the first two touches in the touch array
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Need to find the positions of the touches in the previous frame (A = CurrentPos - DeltaPos)
            Vector2 touchZeroPrevPos = touchZero.position /*Current Position of the touch*/ - touchZero.deltaPosition /*the difference in position between the touchs current position and position last frame*/;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude; // Distance between points on the previous frame
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag; // The change in difference (Positive Results - Zoom Out, Negative Result - Zoom in)

            if (Camera.main.orthographic == true)
            {
                if (rocketControllerScript.HasLaunched)
                {
                    AdjustCamera(deltaMagnitudeDiff * OrthoZoomSpeed, Camera.main.gameObject.transform.localPosition.z, Vector3.zero, true);
                }
                else
                {
                    AdjustCamera(deltaMagnitudeDiff * OrthoZoomSpeed, 0.0f, cameraStartPosPostAnimBehindArrow, false);
                }
            }
        }
        else if (inputScript.CheckIfMouseWheelIsMoving())
        {
            if (Camera.main.orthographic == true)
            {
                if(rocketControllerScript.HasLaunched)
                {
                    AdjustCamera(-Input.mouseScrollDelta.y, Camera.main.gameObject.transform.localPosition.z, Vector3.zero, true);
                }
                else
                {
                    AdjustCamera(-Input.mouseScrollDelta.y, 0.0f, cameraStartPosPostAnimBehindArrow, false);
                }

            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cameraOrthSize"></param>
    /// <param name="zAxis"></param>
    /// <param name="offset"></param>
    /// <param name="local"></param>
    private void AdjustCamera(float cameraOrthSize, float zAxis, Vector3 offset, bool local)
    {
        Camera.main.orthographicSize += cameraOrthSize;
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 12.0f, 55.0f);
        if (local)
        {
            Camera.main.gameObject.transform.localPosition = new Vector3(0.0f, Camera.main.orthographicSize - cameraOrthoStartViewBehindArrow, zAxis);
        }
        else
        {
            Camera.main.gameObject.transform.position = new Vector3(0.0f, Camera.main.orthographicSize - cameraOrthoStartViewBehindArrow, 0.0f) + offset;
        }
    }
    #endregion
}
