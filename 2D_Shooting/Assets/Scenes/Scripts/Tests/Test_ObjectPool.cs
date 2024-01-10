using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_ObjectPool : Test_base
{
    //public ] pools;
    public BulletPool pool1;
    public EnemyPool pool2;
    public BulletEffectPool pool3;
    public ExplosionPool pool4;

    void Start()
    {
        pool1.Initialize();
        pool2.Initialize();
        pool3.Initialize();
        pool4.Initialize();
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Bullet bullet = pool1.GetObject();
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        Enemy enemy = pool2.GetObject();
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        BulletEffect hit = pool3.GetObject();
    }
    protected override void OnTest4(InputAction.CallbackContext context)
    {
        BulletEffect explosion = pool4.GetObject();
    }
}
