using System;
using UnityEngine;

namespace Fighter_scripts
{
    public class ArrowCalculator : MonoBehaviour
    {
        float _maxVelocity;
        
        GetShootAngleResult _getShootAngleResult;

        public float GetMaxVelocity(float maxShootDistance)
        {
            return _maxVelocity = Mathf.Sqrt(maxShootDistance * Physics.gravity.magnitude);
        }

        public struct GetShootAngleResult
        {
            public bool DiscriminantIsNegative;
            public float AngleLow;
            public float AngleHigh;

            public void ClearData()
            {
                DiscriminantIsNegative = false;
                AngleLow = 0;
                AngleHigh = 0;
            }
        }
        
        /// <summary>
        /// Calculating low and high angle of the shot knowing shoot position and target position + max velocity of the arrow
        /// </summary>
        /// <param name="vel">Max arrow velocity</param>
        public GetShootAngleResult GetShootAngle(Vector3 shootPoint, Vector3 targetPoint, float vel)
        {
            _getShootAngleResult.ClearData();
            
            Vector3 vectorToTarget = targetPoint - shootPoint;
            
            //getting distance along XZ plane, it will be our X
            float x = Mathf.Sqrt((vectorToTarget.x * vectorToTarget.x) + (vectorToTarget.z * vectorToTarget.z));
            
            //getting vertical distance, it will be our Y
            float y = -vectorToTarget.y;
            
            float g = Physics.gravity.magnitude;
            
            
            float discriminant = Mathf.Pow(vel, 4) - g * (g * x * x + 2 * vel * vel * y);
            
            if (discriminant < 0)
            {
                _getShootAngleResult.DiscriminantIsNegative = true;
                return _getShootAngleResult;
            }
            else if (discriminant == 0)
            {
                float angle = Mathf.Atan((vel * vel) / (g * x));
                _getShootAngleResult.AngleLow = angle;
                _getShootAngleResult.AngleHigh = angle;
                return _getShootAngleResult;
            }
            else
            {
                float angleLow = Mathf.Atan((vel * vel - Mathf.Sqrt(discriminant)) / (g * x));
                float angleHigh = Mathf.Atan((vel * vel + Mathf.Sqrt(discriminant)) / (g * x));
                _getShootAngleResult.AngleLow = angleLow;
                _getShootAngleResult.AngleHigh = angleHigh;
                return _getShootAngleResult;
            }
        }
        
        /// <summary>
        /// Calculating low and high angle of the shot knowing shoot position and target position + max velocity of the arrow
        /// </summary>
        public float GetShootDistance(Vector3 shootPoint, Vector3 targetPoint, float vel)
        {
            return 0f;
        }
    }
}