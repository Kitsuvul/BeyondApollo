/*
Notes:
 - 

*/
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseShipScript : MonoBehaviour
{
    #region Variables
    private GameObject gameControllerObj, mainCamera, canvasObj;
    private Animator rocketAnimator;
    private TopLevelUIHandler topLevelUIScript;
    private ShipControlsScript shipControlsScript;
    private HelperScript helperScript;
    private bool isDead = false;
    private int endlessScore = 0;
    #endregion

    public bool IsDead
    {
        get { return isDead; }
        set { isDead = value; }
    }

    public int EndlessScore
    {
        get { return endlessScore; }
        private set { endlessScore = value; }
    }

    public Animator RocketAnimator
    {
        get { return rocketAnimator; }
        private set { rocketAnimator = value; }
    }

    #region Unity Functions
    void Awake()
    {
        mainCamera = Camera.main.gameObject;
        gameControllerObj = GameObject.FindGameObjectWithTag("GameController");
        canvasObj = GameObject.FindGameObjectWithTag("CanvasUI");

        helperScript = gameControllerObj.GetComponent<HelperScript>();
        topLevelUIScript = canvasObj.GetComponent<TopLevelUIHandler>();
        shipControlsScript = this.gameObject.GetComponent<ShipControlsScript>();
        rocketAnimator = this.gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        CalculateEndlessScore();
        RotateShipToFaceDirection();
    }
    #endregion

    #region Methods
    /// <summary>
    /// 
    /// </summary>
    private void CalculateEndlessScore()
    {
        if (!isDead && helperScript != null && helperScript.IsEndlessMode())
        {
            endlessScore = Mathf.RoundToInt((helperScript.GetVec2FromPositionHelper(this.transform.position) - shipControlsScript.StartPosition).magnitude);
        }
    }

    /// <summary>
    /// Functions for when the rocket ship collides with a planet or solid object, resets the camera and velocity
    /// </summary>
    public void DestroyShip()
    {
        if(mainCamera && shipControlsScript)
        {
            mainCamera.transform.parent = null;
            shipControlsScript.RemoveVelocityFromShip();
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            isDead = true;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void PostWinAnimation()
    {
        if (topLevelUIScript != null)
        {
            topLevelUIScript.OpenWinStatePanel();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void PostDeathAnimation()
    {
        if (topLevelUIScript != null && helperScript != null)
        {
            if(helperScript.IsEndlessMode())
            {
                topLevelUIScript.OpenEndlessPanel();
            }
            else
            {
                topLevelUIScript.OpenFailStatePanel();
            }
        }
    }

    /// <summary>
    /// Makes the SpaceShip face the direction of travel
    /// </summary>
    public void RotateShipToFaceDirection()
    {
        if (shipControlsScript != null && shipControlsScript.HasLaunched)
        {
            Vector2 currDir = this.gameObject.GetComponent<Rigidbody2D>().linearVelocity;

            if (currDir != Vector2.zero)
            {
                float angle = (Mathf.Atan2(currDir.y, currDir.x) * Mathf.Rad2Deg) - 90.0f;
                this.gameObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnWinAnimation()
    {
        rocketAnimator.SetTrigger("OnWin");
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnDeathAnimation()
    {
        rocketAnimator.SetTrigger("OnDeath");
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnLostAnimation()
    {
        rocketAnimator.SetTrigger("OnLost");
    }
    #endregion
}
