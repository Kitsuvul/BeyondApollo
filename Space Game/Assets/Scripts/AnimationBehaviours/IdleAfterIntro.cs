using UnityEngine;

public class IdleAfterIntro : StateMachineBehaviour
{
    private InputScript inputScript;
    private GameObject rocketShipObj, gameControllerObj, canvasObj;
    private ShipControlsScript rocketControllerScript;

    public CameraScript CameraScript;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rocketShipObj = GameObject.FindGameObjectWithTag("Player");
        gameControllerObj = GameObject.FindGameObjectWithTag("GameController");

        rocketControllerScript = rocketShipObj.GetComponent<ShipControlsScript>();
        inputScript = gameControllerObj.GetComponent<InputScript>();

        CameraScript = Camera.main.GetComponent<CameraScript>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (inputScript.CheckSingleClickAndHold() && !rocketControllerScript.IsTouched && !rocketControllerScript.HasLaunched)
        {
            Debug.Log("Getting There?");
            CameraScript.MoveCamera();
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
