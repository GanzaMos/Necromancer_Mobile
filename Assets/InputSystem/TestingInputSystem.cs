using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestingInputSystem : MonoBehaviour
{
    PlayerInput _playerInput;
    
    // void Awake()
    // {
    //     _playerInput = GetComponent<PlayerInput>();
    //     _playerInput.onActionTriggered += Click;
    // }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Click(InputAction.CallbackContext context)
    {
        Debug.Log(context);
        
        // if (context.phase == InputActionPhase.Started)
        //     Debug.Log("Click!" + context.phase);
    }
}
