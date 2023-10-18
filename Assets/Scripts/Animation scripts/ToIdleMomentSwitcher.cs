using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToIdleMomentSwitcher : StateMachineBehaviour
{
    [SerializeField] float minTimeForIdleMoment = 5;
    [SerializeField] float maxTimeForIdleMoment = 15;
    [SerializeField] string idleMomentTriggerName = "IdleMomentTrigger";
    
    float _timer;
    float _timeToSwitch;
    int _idleMomentTriggerHash = 0;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_idleMomentTriggerHash == 0)
        {
            _idleMomentTriggerHash = Animator.StringToHash(idleMomentTriggerName);
        }

        _timer = 0;
        _timeToSwitch = Random.Range(minTimeForIdleMoment, maxTimeForIdleMoment);
    }

    
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _timer += Time.deltaTime;

        if (_timer > _timeToSwitch)
        {
            animator.SetTrigger(_idleMomentTriggerHash);
            _timer = 0;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}
}
