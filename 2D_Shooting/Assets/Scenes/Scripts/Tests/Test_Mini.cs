using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Mini : Test_base
{
    Transform target;
    Transform spawnPoint;

    private void Start()
    {
        target = transform.GetChild(0);
        spawnPoint = transform.GetChild(1); 
    }
    protected override void OnTest1(InputAction.CallbackContext context)
    {
        AsteroidMini mini =  Factory.Instance.GetAsteroidMini();
        mini.Direction = target.transform.position;
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        Factory.Instance.GetAsteroid(spawnPoint.position);
    }
}
