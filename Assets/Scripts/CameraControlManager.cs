using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControlManager : MonoBehaviour
{
    [SerializeField, Tooltip("Camera move speed")] 
    float moveSpeed = 20.0f;
    
    [SerializeField, Tooltip("Name of the 'Move Camera' action in Input Action map")]
    string moveCameraActionName = "Move Camera";
    
    float _cameraYPos; 
    float _cameraYStartRotation;
    Quaternion _yRotation;
    
    Transform _cameraTransform;
    PlayerInput _playerInput;
    InputAction _moveCamera;


    void Awake()
    {
        _playerInput = GetComponent<PlayerInput>() ?? throw new Exception("Player Input component not found.");
        _moveCamera = _playerInput.actions.FindAction("Move Camera") ?? throw new Exception($"{moveCameraActionName} action not found.");
        _cameraTransform = Camera.main.transform;
    }

    void OnEnable()
    {
        _moveCamera.Enable();
    }

    void OnDisable()
    {
        _moveCamera.Disable();
    }
    
    void Start()
    {
        if (_cameraTransform == null)
        {
            Debug.Log("Main Camera not found");
        }

        _cameraYPos = _cameraTransform.position.y;
        _yRotation = Quaternion.Euler(0, _cameraTransform.rotation.eulerAngles.y, 0);
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
}
