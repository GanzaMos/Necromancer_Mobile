using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationRandomizer : StateMachineBehaviour
{
    [SerializeField] string idleSpeedParameterName;
    [SerializeField] float minSpeed;
    [SerializeField] float maxSpeed;
    
    [Space (5)]
    
    [SerializeField] string idleOffsetParameterName;
    [SerializeField] float maxOffset;

    bool _isHashed = false;
    int _idleSpeedHash;
    int _idleOffsetHash;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        HashingStrings();
        animator.SetFloat(_idleSpeedHash, Random.Range(minSpeed, maxSpeed));
        animator.SetFloat(_idleOffsetHash, Random.Range(0f, maxOffset));
    }

    void HashingStrings()
    {
        if (_isHashed == true) return;

        _idleSpeedHash = Animator.StringToHash(idleSpeedParameterName);
        _idleOffsetHash = Animator.StringToHash(idleOffsetParameterName);
        _isHashed = true;
    }



    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
