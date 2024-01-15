using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Factory : Test_base
{
    public PoolObejctType objectType;
    public Vector3 position = Vector3.zero;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Factory.Instance.GetObject(objectType);
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        switch (objectType) 
        {
            case PoolObejctType.PlayerBullet:
                Factory.Instance.GetBullet(position);
                break;
            case PoolObejctType.Explosion:
                Factory.Instance.GetExplosion(position);
                break;
            case PoolObejctType.EnemyWave:
                Factory.Instance.GetEnemyWave(position);
                break;
            case PoolObejctType.Hit:
                Factory.Instance.GetHitEffect(position);
                break;
        }
    }
}
