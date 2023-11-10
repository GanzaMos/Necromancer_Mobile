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
        GetProjectileTimeResult _getProjectileTimeResult;
        
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
        /// Calculates Low Angle and High Angle of the parabolic uniform projectile shot, knowing Shot Position, Target Position and Velocity of the projectile.
        /// <br/>Angles will work only for ForceMode.VelocityChange mode in <see cref="Rigidbody"/>.AddForce
        /// <br/>Doesn't take onto account <see cref="Rigidbody.drag"/>  parameter. 
        /// </summary>
        /// <param name="vel">Projectile uniform velocity</param>
        /// <returns>
        /// <see cref="GetShootAngleResult.CanReachTheTarget"/> : Can projectile reach the target with current velocity?
        /// <br/> <see cref="GetShootAngleResult.AngleLow"/> : Low Angle trajectory, in radians
        /// <br/> <see cref="GetShootAngleResult.AngleHigh"/>: High Angle trajectory, in radians
        /// </returns>>
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
                _getShootAngleResult.AngleLow = angle;
                _getShootAngleResult.AngleHigh = angle * Mathf.Rad2Deg;
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
        /// Calculating how close the Shooter must be to the Target in XZ plane to hit it with a projectile uniform moving along a parabolic trajectory
        /// <br/>Takes into account Target's Y position relative to the Shooter.
        /// <br/>Doesn't take onto account <see cref="Rigidbody.drag"/>  parameter. 
        /// </summary>
        /// <param name="vel">Projectile uniform velocity</param>
        /// <returns>
        /// <see cref="GetXZDistanceResult.CanReachTheTarget"/>: Can projectile hit the target with the current velocity 
        /// <br/> <see cref="GetXZDistanceResult.Distance"/>: Max distance to shoot the target, in XZ plane, in meters
        /// </returns>>
        public GetXZDistanceResult GetXZDistanceToShoot(Vector3 shooterPosition, Vector3 targetPosition, float vel)
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

        
        public struct GetProjectileTimeResult
        {
            public bool CanReachTheTarget;
            public float Time;

            public void ClearData()
            {
                CanReachTheTarget = true;
                Time = 0;
            }
        }

        /// <summary>
        /// Calculates time to reach the target for the projectile with uniform parabolic trajectory, velocity and angle
        /// </summary>
        /// <param name="vel">Projectile uniform velocity</param>
        /// <param name="angleInRad">Angle to the horizon of shooting, in radians</param>
        /// <param name="useChecks">TRUE will check if the projectile can hit the target with the given velocity or not, FALSE will skip these calculations and
        /// <see cref="GetProjectileTimeResult.CanReachTheTarget"/> will always be TRUE</param>
        /// <returns>
        /// <see cref="GetProjectileTimeResult.CanReachTheTarget"/>: Can projectile hit the target with the current velocity 
        /// <see cref="GetProjectileTimeResult.Time"/>: Time to reach the target, in seconds 
        /// </returns>
        public GetProjectileTimeResult GetFlightTime(Vector3 shooterPosition, Vector3 targetPosition, float vel, float angleInRad, bool useChecks = true)
        {
            _getProjectileTimeResult.ClearData();
            float g = Physics.gravity.magnitude;
            float y = shooterPosition.y - targetPosition.y;
            
            //finding t from Y part of the equation  y = v*sin(a)*t - g*t^2/2
            float sinAngle = Mathf.Sin(angleInRad);
            float discriminant = vel * vel * sinAngle * sinAngle + 2 * g * y;

            //first check
            //check Y axis, is it possible to reach it
            if (discriminant < 0 && useChecks)
            {
                _getProjectileTimeResult.CanReachTheTarget = false;
                return _getProjectileTimeResult;
            }

            float time = (vel * sinAngle + Mathf.Sqrt(discriminant)) / g;
            _getProjectileTimeResult.Time = time;
            
            //second check
            //check X axis, using equation x = v*t*cos(a)
            if (useChecks == true)
            {
                Vector3 vectorToTarget = targetPosition - shooterPosition;
                float xReal = Mathf.Sqrt((vectorToTarget.x * vectorToTarget.x) + (vectorToTarget.z * vectorToTarget.z));
                float xByTime = vel * time * Mathf.Cos(angleInRad);
                if (Mathf.Approximately(xReal, xByTime) == false)
                {
                    _getProjectileTimeResult.CanReachTheTarget = false;
                    return _getProjectileTimeResult;
                }
            }

            //after all checks, return Time of the fly value
            return _getProjectileTimeResult;
        }

        /// <summary>
        /// Calculate target position with some velocity after certain time.
        /// <br/> Doesn't take into account acceleration and turning velocity.
        /// </summary>
        /// <param name="targetPosition">Target position at this moment</param>
        /// <param name="targetVelocity">Target velocity at this moment</param>
        /// <param name="time">Time for prediction, in seconds</param>
        /// <returns>
        /// Future target Vector3 position
        /// </returns>
        public Vector3 GetTargetPositionPrediction(Vector3 targetPosition, Vector3 targetVelocity, float time)
        {
            //in case if velocity = 0
            if (targetVelocity == Vector3.zero)
                return targetPosition;
            
            Vector3 futurePosition = new Vector3();
            return futurePosition = targetPosition + targetVelocity * time;
        }

        /// <summary>
        /// Calculate target velocity, using target position1, target position2 and time required to cover the distance between them.
        /// Helpful when you don't have access to target's Rigidbody velocity.
        /// </summary>
        /// <param name="time">Time needed to reach position2 from position1</param>
        /// <returns>Target velocity at the current moment</returns>
        public Vector3 GetVelocityFromTransformOffset(Vector3 targetPosition1, Vector3 targetPosition2, float time)
        {
            Vector3 offset = targetPosition2 - targetPosition1;
            Vector3 velocity = offset / time;
            return velocity;
        }
    }
}