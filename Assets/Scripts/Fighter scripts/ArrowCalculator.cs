using System;
using UnityEngine;

namespace Fighter_scripts
{
    public class ArrowCalculator
    {
        #region Singleton

        static ArrowCalculator _instance;
        public static ArrowCalculator Instance 
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ArrowCalculator();
                }
                return _instance;
            }
            private set { }
        }

        #endregion
        
        GetShootAngleResult _getShootAngleResult;
        GetXZDistanceResult _getXZDistanceResult;
        
        public float GetMaxVelocity(float maxShootDistance)
        {
             float maxVelocity = Mathf.Sqrt(maxShootDistance * Physics.gravity.magnitude);
             return maxVelocity;
        }

        public struct GetShootAngleResult
        {
            public bool CanReachTheTarget;
            public float AngleLow;
            public float AngleHigh;

            public void ClearData()
            {
                CanReachTheTarget = true;
                AngleLow = 0;
                AngleHigh = 0;
            }
        }
        
        /// <summary>
        /// Calculating Low Angle and High Angle of the Parabolic projectile shot, knowing Shot Position, Target Position and Velocity of the projectile.
        /// <para>Angles will work only for ForceMode.VelocityChange mode in <see cref="Rigidbody"/>.AddForce.</para>
        /// Doesn't take into account Rigidbody Drag parameter.
        /// </summary>
        /// <param name="vel">Max projectile velocity</param>
        /// <returns>Can reach the target at all, Low Angle trajectory, High Angle trajectory</returns>>
        public GetShootAngleResult GetShootAngle(Vector3 shooterPosition, Vector3 targetPosition, float vel)
        {
            _getShootAngleResult.ClearData();
            
            Vector3 vectorToTarget = targetPosition - shooterPosition;
            
            //getting horizontal distance between Shooter and Target along XZ plane, it will be our X
            float x = Mathf.Sqrt((vectorToTarget.x * vectorToTarget.x) + (vectorToTarget.z * vectorToTarget.z));
            
            //getting vertical distance between Shooter and Target, it will be our Y
            float y = vectorToTarget.y;
            
            float g = Physics.gravity.magnitude;
            
            //getting discriminant for our equation, if it's < 0, the Target Position is out of reach
            float discriminant = Mathf.Pow(vel, 4) - g * (g * x * x + 2 * vel * vel * y);
            
            if (discriminant < 0)
            {
                _getShootAngleResult.CanReachTheTarget = false;
                return _getShootAngleResult;
            }
            else if (discriminant == 0)
            {   
                float angle = Mathf.Atan((vel * vel) / (g * x));
                _getShootAngleResult.AngleLow = angle * Mathf.Rad2Deg;
                _getShootAngleResult.AngleHigh = angle * Mathf.Rad2Deg;
                return _getShootAngleResult;
            }
            else
            {
                float angleLow = Mathf.Atan((vel * vel - Mathf.Sqrt(discriminant)) / (g * x));
                float angleHigh = Mathf.Atan((vel * vel + Mathf.Sqrt(discriminant)) / (g * x));
                _getShootAngleResult.AngleLow = angleLow * Mathf.Rad2Deg;
                _getShootAngleResult.AngleHigh = angleHigh * Mathf.Rad2Deg;
                return _getShootAngleResult;
            }
        }
        
        public struct GetXZDistanceResult
        {
            public bool CanReachTheTarget;
            public float Distance;

            public void ClearData()
            {
                CanReachTheTarget = true;
                Distance = 0;
            }
        }
        
        /// <summary>
        /// Calculating how close the shooter must be to the target in XZ plane to hit it with a projectile moving along a parabolic trajectory.
        /// <para>Takes into account target's Y position relative to the shooter.</para>
        /// </summary>
        /// <param name="vel">Max projectile velocity</param>
        /// <returns>Real distance to the target in XZ plane, in meters</returns>>
        public GetXZDistanceResult GetXZDistanceToShootTarget(Vector3 shooterPosition, Vector3 targetPosition, float vel)
        {
            _getXZDistanceResult.ClearData();
            float g = Physics.gravity.magnitude;
            float h = shooterPosition.y - targetPosition.y;

            //projectile velocity at the target position
            //simplified version of kinematic equation mv^2/2 (final) = mv^2/2 (start) + gmh
            float finalVelocitySqrt = vel * vel + 2 * g * h;

            //if finalVelocitySqrt < 0 - projectile can't reach the Target with the current velocity
            if (finalVelocitySqrt < 0)
            {
                _getXZDistanceResult.CanReachTheTarget = false;
                return _getXZDistanceResult;
            }
            
            float finalVelocity = Mathf.Sqrt(finalVelocitySqrt);
            
            //atan of the triangle at the target position, created from Vel start and Vel final
            //inspired by this video - https://www.youtube.com/watch?v=xwhbT9Do1RQ
            float angle = Mathf.Atan(vel /  finalVelocity);

            //using movement of a body thrown at an angle equation
            //finding t from Y part of the equation  y = v*sin(a)*t - g*t^2/2
            float sinAngle = Mathf.Sin(angle);
            float discriminant = vel * vel * sinAngle * sinAngle + 2 * g * h;
            
            float t = (vel * sinAngle + Mathf.Sqrt(discriminant)) / g;
            
            //past in into X part of the equation x = v*t*cos(a)
            float distance = vel * Mathf.Cos(angle) * t;
            _getXZDistanceResult.Distance = distance;
            
            return _getXZDistanceResult;
        }
    }
}