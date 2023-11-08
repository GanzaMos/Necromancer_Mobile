using System;
using System.Collections;
using System.Collections.Generic;
using Fighter_scripts;
using Pool_scripts;
using Unity.VisualScripting;
using UnityEngine;

public class ArrowSpawner : MonoBehaviour
{
    [SerializeField] GameObject arrowPrefab;
    
    [Tooltip("How far the arrow should shoot, in Unity units")]
    [SerializeField] float maxArrowDistance;
    
    [SerializeField] int gizmosNumber;
    [SerializeField] float gizmosDistance;
    
    ArrowCalculator _arrowCalculator;

    float _maxVelocity;
    
    void Awake()
    {
        _arrowCalculator = GetComponent<ArrowCalculator>();
    }

    void Start()
    {
        NullCheck();
        _maxVelocity = _arrowCalculator.GetMaxVelocity(maxArrowDistance);
    }

    void NullCheck()
    {
       
        if (_arrowCalculator == null) 
            Debug.Log($"Can't get {_arrowCalculator.GetType().Name} for {GetType().Name} in {gameObject.name}", gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject arrowInstance = PoolManager.Instance.Get(arrowPrefab);
            SetPositionAndRotation(arrowInstance);
            SetArrowParameters(arrowInstance);
        }
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
        arrowHandler.LaunchArrow();
    }
    
    private void OnDrawGizmos()
    {
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
