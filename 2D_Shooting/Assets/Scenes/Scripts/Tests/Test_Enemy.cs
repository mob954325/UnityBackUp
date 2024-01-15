using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Enemy : Test_base
{
    public PoolObejctType objectType;
    Transform spawn;

    void Start()
    {
        spawn = transform.GetChild(0);
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Factory.Instance.GetEnemyWave(spawn.position);
    }
 }
