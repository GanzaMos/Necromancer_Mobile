using Pool_scripts;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class ArrowHandler : MonoBehaviour
{
    [SerializeField] Transform torquePart;
    [SerializeField] float torqueForceMin = 2f;
    [SerializeField] float torqueForceMax = 10f;
    float _torqueForce;
    
    [SerializeField] string tagToHit;
    
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
        NullCheck();
    }

    void NullCheck()
    {
        if (_arrowRigidbody == null) Debug.Log($"Can't get {_arrowRigidbody.GetType().Name} for {GetType().Name} in {gameObject.name}", gameObject);
        if (_launchPoint == null)    Debug.Log($"Can't get {_launchPoint.GetType().Name} for {GetType().Name} in {gameObject.name}", gameObject);
        if (torquePart == null)      Debug.Log($"Can't get {_launchPoint.GetType().Name} for {GetType().Name} in {gameObject.name}", gameObject);
    }

    void FixedUpdate()
    {
        if (_isFlying == false) return;
        
        AdjustArrowAngle();
        RotateArrowZAxis();
    }
    
    public void LaunchArrow(float launchForce)
    {
        _isFlying = true;
        _arrowRigidbody.isKinematic = false;
        _torqueForce = Random.Range(torqueForceMin, torqueForceMax);
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
            
            PoolManager.Instance.Release(gameObject);
        }
    }
    
   

}
