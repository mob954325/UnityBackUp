using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Test_FactoryRefactoring : Test_base
{
    public PoolObejctType objectType;

    [Range(0,360.0f)]
    public float angle;

    Transform target;
    Transform spawnPoint;

    private void Start()
    {
        target = transform.GetChild(0);
        spawnPoint = transform.GetChild(1); 
    }
    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Factory.Instance.GetObject(objectType, spawnPoint.position, new Vector3(0,0,angle));
    }
}
