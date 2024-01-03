using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_base : MonoBehaviour
{
    Test_InputAction inputAction;

    void Awake()
    {
        inputAction = new Test_InputAction();
    }

    void OnEnable()
    {
        inputAction.Test.Enable();
        inputAction.Test.Test1.performed += OnTest1;
        inputAction.Test.Test2.performed += OnTest2;
        inputAction.Test.Test3.performed += OnTest3;
        inputAction.Test.Test4.performed += OnTest4;
        inputAction.Test.Test5.performed += OnTest5;
        inputAction.Test.L_Click.performed += OnLClick;
        inputAction.Test.R_Click.performed += OnRClick;
    }


    void OnDisable()
    {
        inputAction.Test.R_Click.performed -= OnRClick;
        inputAction.Test.L_Click.performed -= OnLClick;
        inputAction.Test.Test5.performed -= OnTest5;
        inputAction.Test.Test4.performed -= OnTest4;
        inputAction.Test.Test3.performed -= OnTest3;
        inputAction.Test.Test2.performed -= OnTest2;
        inputAction.Test.Test1.performed -= OnTest1;
        inputAction.Test.Disable();
    }

    protected virtual void OnRClick(InputAction.CallbackContext context)
    {
        
    }

    protected virtual void OnLClick(InputAction.CallbackContext context)
    {
        
    }

    protected virtual void OnTest5(InputAction.CallbackContext context)
    {
        
    }

    protected virtual void OnTest4(InputAction.CallbackContext context)
    {
        
    }

    protected virtual void OnTest3(InputAction.CallbackContext context)
    {
        
    }

    protected virtual void OnTest2(InputAction.CallbackContext context)
    {
        
    }

    protected virtual void OnTest1(InputAction.CallbackContext context)
    {
        
    }

}
