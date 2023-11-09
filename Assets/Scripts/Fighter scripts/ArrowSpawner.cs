using System;
using System.Collections;
using System.Collections.Generic;
using Fighter_scripts;
using Pool_scripts;
using Unity.VisualScripting;
using UnityEngine;

public class ArrowSpawner : MonoBehaviour
{
    [Header ("Arrow properties")]
    
    [SerializeField] GameObject arrowPrefab;
    
    [Tooltip("How far the arrow should shoot, in Unity units"), SerializeField] 
    float maxArrowDistance;

    [Header("Debugger"), Space(5)] 
    [SerializeField] bool showTrajectory;
    [SerializeField] int gizmosNumber;
    [SerializeField] float gizmosDistance;

    [SerializeField] bool useHighTrajectory;
    [SerializeField] Transform testTarget1;
    [SerializeField] Transform testTarget2;
    [SerializeField] Transform testTarget3;

    Transform _thisTransform;
    [SerializeField] float _maxVelocity;

    void Awake()
    {
        _thisTransform = transform;
    }
    
    void Start()
    {
        _maxVelocity = ArrowCalculator.Instance.GetMaxVelocity(maxArrowDistance);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) ShootArrow(testTarget1);
        if (Input.GetKeyDown(KeyCode.Alpha2)) ShootArrow(testTarget2);
        if (Input.GetKeyDown(KeyCode.Alpha3)) ShootArrow(testTarget3);
    }

    void ShootArrow(Transform target)
    {
        transform.LookAt(new Vector3(target.position.x, 0, target.position.z));
        if (SetSpawnerXAngleToShoot(target) == false) return;
        GameObject arrowInstance = PoolManager.Instance.Get(arrowPrefab);
        SetPositionAndRotation(arrowInstance);
        SetArrowParameters(arrowInstance);
    }

    bool SetSpawnerXAngleToShoot(Transform target)
    {
        ArrowCalculator.GetShootAngleResult angleResult = 
            ArrowCalculator.Instance.GetShootAngle(transform.position, target.position, _maxVelocity);

        if (angleResult.DiscriminantIsNegative == true)
        {
            Debug.Log("Can't reach the target, discriminant is negative");
            return false;
        }

        if (useHighTrajectory == true)
        {
            float highAngle = angleResult.AngleHigh;
            SetXRotation(highAngle);
            return true;
        }
        else
        {
            float lowAngle = angleResult.AngleLow;
            SetXRotation(lowAngle);
            return true;
        }
    }

    void SetXRotation(float angle)
    {
        transform.rotation = Quaternion.Euler(-angle, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }

    void SetPositionAndRotation(GameObject arrowInstance)
    {
        Transform arrowInstanceTransform = arrowInstance.transform;
        arrowInstanceTransform.position = transform.position;
        arrowInstanceTransform.rotation = transform.rotation;
    }

    void SetArrowParameters(GameObject arrowInstance)
    {
        ArrowHandler arrowHandler = arrowInstance.GetComponent<ArrowHandler>();
        arrowHandler.LaunchArrow(_maxVelocity);
    }
    
    void OnDrawGizmos()
    {
        if (showTrajectory)
            DrawTrajectoryPrediction();
    }

    void DrawTrajectoryPrediction()
    {
        Vector3 initialPosition = transform.position;
        Vector3 initialVelocity = transform.forward * _maxVelocity;

        for (int i = 0; i < gizmosNumber; i++)
        {
            float time = i * gizmosDistance;
            Vector3 position = CalculateProjectilePosition(initialPosition, initialVelocity, time);

            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(position, 0.05f);
        }
    }

    Vector3 CalculateProjectilePosition(Vector3 initialPosition, Vector3 initialVelocity, float time)
    {
        Vector3 gravity = Physics.gravity;

        // Projectile motion equation
        Vector3 position = initialPosition + initialVelocity * time + 0.5f * gravity * time * time;

        return position;
    }
}
