using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimHashBase
{
    static bool _isInitialized = false;

    public static int AttackAnimationNumber;
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Initialize()
    {
        if (_isInitialized) return;
        
        AttackAnimationNumber = Animator.StringToHash("AttackAnimationNumber");
        
        _isInitialized = true;
    }
}
