using System;
using System.Collections;
using System.Collections.Generic;
using Pool_scripts;
using UnityEngine;
using Random = UnityEngine.Random;

public class ArrowHandler : MonoBehaviour
{
    [SerializeField] float launchForce = 10f;
    
    [SerializeField] Transform torquePart;
    [SerializeField] float torqueForceMin = 2f;
    [SerializeField] float torqueForceMax = 10f;
    float _torqueForce;
    
    [SerializeField] string tagToHit;

    [SerializeField] int gizmosNumber;
    [SerializeField] float gizmosDistance;

    public PoolManager PoolManager { get; set; }
    Rigidbody _arrowRigidbody;
    Transform _launchPoint;

    bool _isFlying = false;
    

    void Awake()
    {
        _arrowRigidbody = GetComponent<Rigidbody>();
        _launchPoint = GetComponent<Transform>();
    }

    void Start()
    {
        if (_arrowRigidbody == null) Debug.Log($"Can't get {_arrowRigidbody.GetType().Name} for {GetType().Name} in {gameObject.name}", gameObject);
        if (_launchPoint == null)    Debug.Log($"Can't get {_launchPoint.GetType().Name} for {GetType().Name} in {gameObject.name}", gameObject);
        if (torquePart == null)      Debug.Log($"Can't get {_launchPoint.GetType().Name} for {GetType().Name} in {gameObject.name}", gameObject);

        _torqueForce = Random.Range(torqueForceMin, torqueForceMax);
    }
    
    void FixedUpdate()
    {
        if (_isFlying == false) return;
        
        AdjustArrowAngle();
        RotateArrowZAxis();
    }
    
    public void LaunchArrow()
    {
        _isFlying = true;
        _arrowRigidbody.isKinematic = false;
        _arrowRigidbody.AddForce(_launchPoint.forward * launchForce, ForceMode.VelocityChange);
    }
    
    void AdjustArrowAngle()
    {
        if (_arrowRigidbody.velocity.sqrMagnitude != 0)
        {
            Vector3 arrowDirection = _arrowRigidbody.velocity.normalized;
            Vector3 localArrowDirection = _launchPoint.InverseTransformPoint(arrowDirection);
            transform.forward = arrowDirection;
        }
    }

    void RotateArrowZAxis()
    {
        torquePart.localRotation *= Quaternion.Euler(0f, 0f, _torqueForce);
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag(tagToHit))
        {
            _isFlying = false;
            _arrowRigidbody.isKinematic = true;
            gameObject.transform.SetParent(collision.gameObject.transform);
            
            PoolManager.Release(gameObject);
        }
    }
    
    // private void OnDrawGizmos()
    // {
    //     DrawTrajectoryPrediction();
    // }

    void DrawTrajectoryPrediction()
    {
        Vector3 initialPosition = transform.position;
        Vector3 initialVelocity = transform.forward * launchForce;

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
