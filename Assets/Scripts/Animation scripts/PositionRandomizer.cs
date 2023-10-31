using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PositionRandomizer : StateMachineBehaviour
{
    [SerializeField] string хParameterName;
    [SerializeField] float minX;
    [SerializeField] float maxX;
    int _хHash;
    
    [Space(5f)]
    [SerializeField] string yParameterName;
    [SerializeField] float minY;
    [SerializeField] float maxY;
    int _yHash;

    [Tooltip("Activate if you want to keep X and Y parameters same each transition to this Clip")]
    [SerializeField] bool randomizeEveryClipStart = true;
    
    bool _isHashed = false;
    bool _alreadyRandomized = false;
    
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        HashingStrings();
        
        if (randomizeEveryClipStart == false && _alreadyRandomized == true) return;
        RandomizeXY(animator);
        _alreadyRandomized = true;
    }
    
    void HashingStrings()
    {
        if (_isHashed == true) return;

        _хHash = Animator.StringToHash(хParameterName);
        _yHash = Animator.StringToHash(yParameterName);
        _isHashed = true;
    }

    void RandomizeXY(Animator animator)
    {
        animator.SetFloat(_хHash, Random.Range(minX, maxX));
        animator.SetFloat(_yHash, Random.Range(minY, maxY));
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
