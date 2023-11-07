using UnityEngine;
using UnityEngine.Animations.Rigging;

public class BowIKHandler : MonoBehaviour
{
    [SerializeField] UnityEngine.GameObject upperLimbIK;
    [SerializeField] UnityEngine.GameObject lowerLimbIK;
    [SerializeField] UnityEngine.GameObject stringIK;
    [SerializeField] UnityEngine.GameObject mainBoneIK;

    float _targetStringWeight, _stringTimer, _stringDuration;
    float _angleWeight,  _targetAngleWeight,  _angleTimer,  _angleDuration;
    float _upperLimbIKWeight, _lowerLimbIKWeight;
    
    //Bow IK constraints references (for string, limbs, local rotation in hand)
    MultiAimConstraint _angleConstraint;
    ChainIKConstraint _upperLimbConstraint;
    ChainIKConstraint _lowerLimbConstraint;
    MultiPositionConstraint _stringSources;
    
    void Awake()
    {
        //getting bow's Main Bone constraint so we can change its angle
        _angleConstraint = mainBoneIK.GetComponent<MultiAimConstraint>();
        
        //getting bow's Lower and Upper Limbs constraints, so it will bend during bow drawing
        _upperLimbConstraint = upperLimbIK.GetComponent<ChainIKConstraint>();
        _lowerLimbConstraint = lowerLimbIK.GetComponent<ChainIKConstraint>();
        
        //getting bow's String and Origin Point sources, so it can draws and release string after shoot
        _stringSources = stringIK.GetComponent<MultiPositionConstraint>();
    }


    void Update()
    {
        AdjustString();
        AdjustAngle();
    }

    
    void AdjustString()
    {
        if (_targetStringWeight != _stringSources.data.sourceObjects.GetWeight(0))
        {
            _stringTimer += Time.deltaTime;
            float timerPercent = Mathf.Clamp01(_stringTimer / _stringDuration);

            if (_targetStringWeight == 1)
            {
                var sources = _stringSources.data.sourceObjects;
                sources.SetWeight(0, Mathf.Lerp(0, 1, timerPercent));
                sources.SetWeight(1, Mathf.Lerp(1, 0, timerPercent));
                _stringSources.data.sourceObjects = sources;
                
                _upperLimbConstraint.weight = Mathf.Lerp(0, 0.3f, timerPercent);
                _lowerLimbConstraint.weight = Mathf.Lerp(0, 0.3f, timerPercent);
            }
            else if (_targetStringWeight == 0)
            {
                var sources = _stringSources.data.sourceObjects;
                sources.SetWeight(0, Mathf.Lerp(1, 0, timerPercent));
                sources.SetWeight(1, Mathf.Lerp(0, 1, timerPercent));
                _stringSources.data.sourceObjects = sources;
                
                _upperLimbConstraint.weight = Mathf.Lerp(0.3f, 0, timerPercent);
                _lowerLimbConstraint.weight = Mathf.Lerp(0.3f, 0, timerPercent);
            }
        }
    }

    
    void AdjustAngle()
    {
        if (_targetAngleWeight != _angleConstraint.weight)
        {
            _angleTimer += Time.deltaTime;
            float timerPercent = Mathf.Clamp01(_angleTimer / _angleDuration);

            if (_targetAngleWeight == 1)
                _angleConstraint.weight = Mathf.Lerp(0f, 1, timerPercent);
            else if (_targetAngleWeight == 0)
                _angleConstraint.weight = Mathf.Lerp(1f, 0, timerPercent);
        }
    }

    //attack animation driven 
    void StartBowAngleAdjust(int frameToIncrease) =>  SetAngleIK(frameToIncrease, 1);
    void StopBowAngleAdjust(int frameToDecrease)  =>  SetAngleIK(frameToDecrease, 0);
    void StringStartDrawing(int frameToIncrease)  =>  SetStringIK(frameToIncrease, 1);
    void StringStopDrawing(int frameToDecrease)   =>  SetStringIK(frameToDecrease, 0);

    void SetAngleIK(int frames, float targetValue)
    {
        _angleDuration = frames / 60f; 
        _targetAngleWeight = targetValue;
        _angleTimer = 0;
    }

    void SetStringIK(int frames, float targetValue)
    {
        _stringDuration = frames / 60f; 
        _targetStringWeight = targetValue;
        _stringTimer = 0;
    }
}
