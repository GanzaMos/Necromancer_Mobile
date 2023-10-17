using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class ChController : MonoBehaviour
{
    Camera       _mainCamera;
    NavMeshAgent _navMeshAgent;
    Inputs       _inputs;
    Vector2      _lastMovementInput;
    Transform    _target;
    Quaternion   _targetRotation;

    void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _target = GetComponent<Transform>();
        _mainCamera = Camera.main;
        _inputs = new Inputs();
    }

    void OnEnable()
    {
        _inputs.Test.Enable();
    }

    void OnDisable()
    {
        _inputs.Test.Disable();
        _inputs.Test.MouseClick.started -= OnMouseClick;
        
    }

    void Start()
    {
        _inputs.Test.Enable();
        _inputs.Test.MouseClick.started += OnMouseClick;
    }

    void FixedUpdate()
    {

    }

    void OnMouseClick(InputAction.CallbackContext context)
    {
        Vector2 mousePosition = _inputs.Test.MousePosition.ReadValue<Vector2>();

        Ray ray = _mainCamera.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            // Move the player to the clicked point on the ground
            Move(hit.point);
        }
    }
    
    void Move(Vector3 targetPosition)
    {
        // Move the player using NavMeshAgent
        _navMeshAgent.SetDestination(targetPosition);
    }
}