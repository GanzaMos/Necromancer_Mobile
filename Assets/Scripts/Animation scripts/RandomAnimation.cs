using Unity.VisualScripting;
using UnityEngine;

public class RandomAnimation : StateMachineBehaviour
{
    [SerializeField] int numberOfRandomAnimations;
    [SerializeField] string animationParameterName;

    [SerializeField] bool allowDecimalNumbers = false;
    
    int _animationHash;
    bool _isHashed = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_isHashed == false)
        {
            _animationHash = Animator.StringToHash(animationParameterName);
            _isHashed = true;
        }
        
        if (allowDecimalNumbers == false)
            animator.SetFloat(_animationHash, Random.Range(1, numberOfRandomAnimations + 1));
        else
            animator.SetFloat(_animationHash, Random.Range(1f, (float)numberOfRandomAnimations + 1f));
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
