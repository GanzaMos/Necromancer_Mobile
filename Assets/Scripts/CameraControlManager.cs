using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControlManager : MonoBehaviour
{
    [Header ("Camera Move Options"), Space (5)]
    [SerializeField] float moveSpeed = 20;

    [SerializeField] float acceleration = 10;
    
    [SerializeField] float damping = 15;

    [Header ("Zoom options"), Space (5)]
    
    [SerializeField, Tooltip("Closest zoom distance, must be >0")]
    float closestZoomSize = 3;
    
    [SerializeField, Tooltip("Farthest zoom distance, must be >0")]
    float farthestZoomSize = 20;

    [SerializeField] float zoomSpeed = 5;
    [SerializeField] float zoomDamping = 7.5f;
    
    
    float _cameraYPos; 
    float _cameraYStartRotation;
    Quaternion _yRotation;

    Camera _camera;
    Transform _cameraTransform;
    PlayerInput _playerInput;
    InputAction _moveCamera;
    InputAction _primaryFingerPosition;
    InputAction _secondaryFingerPosition;

    Coroutine _zoomCoroutine;

    void Awake()
    {
        _camera = Camera.main;
        _cameraTransform = Camera.main.transform;
        _playerInput = GetComponent<PlayerInput>() ?? throw new Exception("Player Input component not found.");
        _moveCamera = _playerInput.actions.FindAction("Move Camera");
        _primaryFingerPosition = _playerInput.actions.FindAction("PrimaryFingerPosition");
        _secondaryFingerPosition = _playerInput.actions.FindAction("SecondaryFingerPosition");
    }

    void OnEnable()
    {
        _moveCamera.Enable();
        _primaryFingerPosition.Enable();
        _secondaryFingerPosition.Enable();
    }

    void OnDisable()
    {
        _moveCamera.Disable();
        _primaryFingerPosition.Disable();
        _secondaryFingerPosition.Disable();
    }
    
    void Start()
    {
        if (_cameraTransform == null)          Debug.Log("Main Camera not found");
        if (_moveCamera == null)               Debug.Log("MoveCamera action not found in Input Actions");
        if (_primaryFingerPosition == null)    Debug.Log("PrimaryFingerPosition action not found in Input Actions");
        if (_secondaryFingerPosition == null)  Debug.Log("SecondaryFingerPosition action not found in Input Actions");

        //Store initial Camera height and global Y axis rotation
        _cameraYPos = _cameraTransform.position.y;
        _yRotation = Quaternion.Euler(0, _cameraTransform.rotation.eulerAngles.y, 0);
        
        //Activate function when 1) second finger touches the screen and 2) when it stops touching
        _playerInput.actions.FindAction("SecondaryTouchContact").started += _ => ZoomStart();
        _playerInput.actions.FindAction("SecondaryTouchContact").canceled += _ => ZoomEnd();
    }
    
    void Update()
    {
        MoveCamera();
    }

    void MoveCamera()
    {
        Vector2 inputVector = _moveCamera.ReadValue<Vector2>();
        
        Vector3 moveDirectionX = new Vector3(inputVector.x, 0, 0);
        Vector3 moveDirectionY = _yRotation * new Vector3(0, 0, inputVector.y);
        
        Vector3 moveX = moveDirectionX.normalized * (moveSpeed * Time.deltaTime);
        Vector3 moveY = moveDirectionY.normalized * (moveSpeed * Time.deltaTime);
        
        _cameraTransform.Translate(moveX, Space.Self);
        _cameraTransform.Translate(moveY, Space.World);
    }

    void ZoomStart()
    {
        _zoomCoroutine = StartCoroutine(ZoomDetection());
    }

    void ZoomEnd()
    {
        StopCoroutine(_zoomCoroutine);
    }

    IEnumerator ZoomDetection()
    {
        float previousDistance = 0, distance = 0;
        while (true)
        {
            //check current distance
            distance = Vector2.Distance(_primaryFingerPosition.ReadValue<Vector2>(), _secondaryFingerPosition.ReadValue<Vector2>());

            //TODO Check if the fingers move to each other or away from each other
            
            //calculate how far we need to change camera Size value for zooming
            float cameraSizeChange = (distance - previousDistance) * zoomSpeed * Time.deltaTime;
            
            //deduct this value from current Size, clamp it
            _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize - cameraSizeChange, closestZoomSize, farthestZoomSize);

            previousDistance = distance;
        }

        yield return null;
    }

}
