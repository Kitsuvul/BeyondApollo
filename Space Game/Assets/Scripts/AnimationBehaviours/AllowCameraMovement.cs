using UnityEngine;

public class AllowCameraMovement : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Camera.main.GetComponent<CameraScript>().AllowCameraMovement();
        animator.cullingMode = AnimatorCullingMode.CullCompletely;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    Debug.Log("Is Updating");
    //    if (animator.GetBool("Transition") == true)
    //    {
    //        Debug.Log("LetsGOOOOO");
    //        animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
    //        animator.SetTrigger("HasLaunched");
    //    }
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    Debug.Log("Triggered");
    //    animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
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
