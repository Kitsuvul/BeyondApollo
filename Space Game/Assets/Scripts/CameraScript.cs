/*
Notes:


*/
using System.Collections;
using System.Xml;
using UnityEngine;
using UnityEngine.Analytics;

public class CameraScript : MonoBehaviour {

    #region Private Variables
    private float OrthoZoomSpeed = 0.1f;
    private float cameraStartOrthSize = 50.0f;
    private bool rotateCam = false;
    private Vector3 cameraStartPos = new Vector3(0.0f, 47.5f, -10.0f);
    private GameObject rocketShipObj, gameControllerObj, canvasObj;
    private ShipControlsScript rocketControllerScript;
    private BaseShipScript rocketBaseScript;
    private LevelLoader levelLoaderScript;
    private InputScript inputScript;
    private HelperScript helperScript;
    private Animator cameraAnimator;
    private TopLevelUIHandler uiScript;
    private TutorialHandler tutorialScript;
    private Animation cameraAnim;

    private bool enableCameraMovement = false;
    private bool canResetCameraToLaunchPosition = false;

    // New Variables for behind Arrow
    private Vector3 cameraLaunchPosition = new Vector3(0.0f, 10f, -10.0f);
    private float cameraLaunchOrthSize = 20f;

    private Vector3 cameraFollowPosition = new Vector3(0.0f, 0.0f, -14.6f);
    private bool cameraInLaunchPosition = false;
    #endregion

    #region Properties
    public bool CameraInLaunchPosition
    {
        get { return cameraInLaunchPosition; }
        private set { cameraInLaunchPosition = value; }
    }

    public bool EnableCameraMovement
    {
        get { return enableCameraMovement; }
        private set { enableCameraMovement = value; }
    }

    public bool CanResetCameraToLaunchPosition
    {
        get { return canResetCameraToLaunchPosition; }
        private set { canResetCameraToLaunchPosition = value; }
    }
    #endregion

    #region Unity Functions
    void Awake()
    {
        // Objects
        canvasObj = GameObject.FindGameObjectWithTag("CanvasUI");
        rocketShipObj = GameObject.FindGameObjectWithTag("Player");
        gameControllerObj = GameObject.FindGameObjectWithTag("GameController");

        // Scripts
        inputScript = gameControllerObj.GetComponent<InputScript>();
        helperScript = gameControllerObj.GetComponent<HelperScript>();
        levelLoaderScript = gameControllerObj.GetComponent<LevelLoader>();
        uiScript = canvasObj.GetComponent<TopLevelUIHandler>();
        tutorialScript = canvasObj.GetComponent<TutorialHandler>();
        rocketControllerScript = rocketShipObj.GetComponent<ShipControlsScript>();
        rocketBaseScript = rocketShipObj.GetComponent<BaseShipScript>();

        // Unity Components
        cameraAnimator = this.GetComponent<Animator>();
        if(tutorialScript)
        {
            cameraAnim = this.gameObject.GetComponent<Animation>();
        }
    }

    void Update()
    {
        if (tutorialScript || !uiScript.UIIsOpen)
        {
            CheckCameraInLaunchPosition();
            if (EnableCameraMovement && helperScript.IsStoryMode())
            {
                UpdateZoom();
                Camera.main.gameObject.transform.position = new Vector3(Mathf.Clamp(Camera.main.gameObject.transform.position.x, -110.0f + Camera.main.orthographicSize, 110.0f - Camera.main.orthographicSize), Mathf.Clamp(Camera.main.gameObject.transform.position.y, -20.0f + Camera.main.orthographicSize, 160.0f - Camera.main.orthographicSize), -10.0f);

                if (!rocketControllerScript.IsTouched && !rocketControllerScript.HasLaunched)
                {
                    MoveCamera();
                }
            }

            if (rocketControllerScript.HasLaunched)
            {
                UpdateCameraPositionOnLaunch();
                OnLaunchAnimation();
            }

            if (levelLoaderScript && levelLoaderScript.LevelLoaded && !cameraAnimator.GetBool("LevelLoaded"))
            {
                if(this.gameObject.transform.position == cameraStartPos)
                {
                    OnLevelLoadedAnimation();
                }
                else
                {
                    this.gameObject.transform.position = cameraStartPos;
                }

            }

            if(rocketBaseScript.IsDead)
            {
                Debug.Log("IsDead");
                OnDeathAnimation();
            }
        }

        if(rotateCam)
        {
            RotateCamera();
        }
    }
    #endregion

    #region Methods

    public void AllowCameraMovement()
    {
        EnableCameraMovement = true;
    }

    private void CheckCameraInLaunchPosition()
    {
        if(this.gameObject.transform.position != cameraLaunchPosition)
        {
            CameraInLaunchPosition = false;
            return;
        }

        CameraInLaunchPosition = true;
    }


    /// <summary>
    /// Resets the camera to it's original starting point
    /// </summary>
    public void ResetCamera()
    {
        Debug.Log("Reset");
        rotateCam = false;
        ResetAnimationLoop();
        enableCameraMovement = false;
        this.gameObject.transform.parent = null;
        Camera.main.orthographicSize = cameraStartOrthSize;
        this.gameObject.transform.position = cameraStartPos;
        this.gameObject.transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
        cameraAnimator.SetTrigger("Reset");
    }

    /// <summary>
    /// 
    /// </summary>
    public void ResetCameraToLaunchPosition()
    {
        Camera.main.orthographicSize = cameraLaunchOrthSize;
        this.gameObject.transform.position = cameraLaunchPosition;
        this.gameObject.transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
    }

    /// <summary>
    /// Plays a animation for the camera to rotate and lock with the rocket ship
    /// </summary>
    private void UpdateCameraPositionOnLaunch()
    {
        float magnitude = (helperScript.GetVec2FromPositionHelper(rocketShipObj.transform.position) - rocketControllerScript.StartPosition).magnitude;
        if (magnitude >= 5.5f)
        {
            this.transform.parent = rocketShipObj.transform;
            rotateCam = true;
            OnLaunchAnimation();
        }
    }

    private void RotateCamera()
    {
        if (this.transform.rotation == rocketShipObj.transform.rotation && this.transform.localPosition == cameraFollowPosition)
        {
            rotateCam = false;
            return;
        }

        if (this.transform.rotation != rocketShipObj.transform.rotation || this.transform.localPosition != cameraFollowPosition)
        {
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rocketShipObj.transform.rotation, Time.deltaTime * 5);
            this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, cameraFollowPosition, Time.deltaTime * 40);
        }
    }


    public void MoveCamera()
    {
        if(inputScript.CheckSingleTouchAndHold() && Input.touchCount == 1)
        {
            if (Input.GetTouch(0).deltaPosition != Vector2.zero)
            {
                Vector2 prevPos = Input.GetTouch(0).deltaPosition / 50;
                Debug.Log(Input.GetTouch(0).deltaPosition);
                Camera.main.gameObject.transform.localPosition = new Vector3(Camera.main.gameObject.transform.localPosition.x - prevPos.x, Camera.main.gameObject.transform.localPosition.y - prevPos.y, Camera.main.gameObject.transform.position.z);
            }
        }
        else if(inputScript.CheckSingleClickAndHold())
        {
            if (Input.mousePositionDelta != Vector3.zero)
            {
                Vector2 prevPos = Input.mousePositionDelta / 5;
                Camera.main.gameObject.transform.localPosition = new Vector3(Camera.main.gameObject.transform.localPosition.x - prevPos.x, Camera.main.gameObject.transform.localPosition.y - prevPos.y, Camera.main.gameObject.transform.position.z);
            }
        }
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
                    AdjustCamera(deltaMagnitudeDiff * OrthoZoomSpeed, 0.0f, cameraLaunchPosition, false);
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
                    AdjustCamera(-Input.mouseScrollDelta.y, 0.0f, cameraLaunchPosition, false);
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
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 12.0f, 80.0f);
    }

    public void OnLevelLoadedAnimation()
    {
        //cameraAnimator.SetTrigger("LevelLoaded");
        cameraAnimator.SetBool("LevelLoaded", true);
        //hasAnimatedLevelLoad = true;
        cameraAnimator.ResetTrigger("Reset");
    }

    public void OnLaunchAnimation()
    {
        if (!tutorialScript)
        { 
            enableCameraMovement = false;
            cameraAnimator.SetTrigger("HasLaunched");
            cameraAnimator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
        }
    }

    public void OnDeathAnimation()
    {
        cameraAnimator.SetTrigger("HasDied");
    }

    public void OnWinAnimation()
    {
        cameraAnimator.SetTrigger("HasWon");
    }

    public void ResetAnimationLoop()
    {
        cameraAnimator.SetTrigger("Reset");
        cameraAnimator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
        cameraAnimator.ResetTrigger("HasWon");
        cameraAnimator.ResetTrigger("HasDied");
        cameraAnimator.ResetTrigger("HasLaunched");
        //cameraAnimator.ResetTrigger("LevelLoaded");
        cameraAnimator.SetBool("LevelLoaded", true);
        //hasAnimatedLevelLoad = false;
    }

    public void PlayOnLevelLoadAnimation()
    {
        cameraAnim["Camera_IntroShotBehindArrow"].wrapMode = WrapMode.Once;
        cameraAnim.Play("Camera_IntroShotBehindArrow");
        //HasAnimatedIntro = true;
    }
    #endregion
}
