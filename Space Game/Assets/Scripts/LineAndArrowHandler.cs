/*
Notes:

*/
using UnityEngine;

public class LineAndArrowHandler : MonoBehaviour
{
    #region Private Variables
    private LineRenderer launchLine;
    private ShipControlsScript controlsScript;
    private HelperScript helperScript;
    private GameObject player, gameController;

    private bool isPCControls = false;
    private bool hasBeenSetup = false;
    #endregion

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gameController = GameObject.FindGameObjectWithTag("GameController");
        helperScript = gameController.GetComponent<HelperScript>();
        launchLine = this.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleLineDrawing();
    }

    public void SetUpLine(bool isPC, ShipControlsScript shipControlsScript)
    {
        controlsScript = shipControlsScript;
        isPCControls = isPC;

        launchLine.enabled = true;
        launchLine.positionCount = 3;

        hasBeenSetup = true;
    }

    private bool CheckTouchPosition(Vector3 rocketPosition, Vector3 touchPosition, float dist)
    {
        if(rocketPosition.y + dist < touchPosition.y)
        {
            return true;
        }
        return false;
    }
    
    private void DrawLine(Vector2 inputPos)
    {
        Vector2 clampedPos = new Vector2(Mathf.Clamp(inputPos.x, -8f, 8f), Mathf.Clamp(inputPos.y, -8.0f, -1.0f));
        this.gameObject.transform.position = helperScript.GetReverseVector2(clampedPos);

        launchLine.SetPosition(0, clampedPos);
        launchLine.SetPosition(1, player.transform.position);
        launchLine.SetPosition(2, this.gameObject.transform.position);

        if (CheckTouchPosition(player.transform.position, inputPos, 2.5f))
        {
            controlsScript.ResetTouch();
            Destroy(this.gameObject);
            Debug.Log("Destroyed");
        }
    }

    private void HandleLineDrawing()
    {
        if (hasBeenSetup)
        {
            if (isPCControls)
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                DrawLine(mousePos);
            }
            else
            {
                Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                DrawLine(touchPos);
            }

            this.gameObject.transform.rotation = helperScript.CalculateRotationHelper(this.gameObject.transform.position, player.transform.position);

            if (controlsScript)
            {
                if (controlsScript.HasLaunched == true)
                {
                    launchLine.enabled = false;
                    Destroy(this.gameObject);
                    Debug.Log("Destroyed");
                }
            }
            else
            {
                Debug.LogWarning("Can't find the controlsScript");
            }
        }
    }
}
