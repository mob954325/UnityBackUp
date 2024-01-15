using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Asteroid : Test_base
{
    public Transform target;
    public Asteroid asteriod;

    void Start()
    {
        target = transform.GetChild(0);
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        asteriod.SetDestination(target.position);
        
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        Factory.Instance.GetAsteroid();

    }
}
