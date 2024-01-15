using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_ObjectPool : Test_base
{
    //public ] pools;
    public BulletPool pool1;
    public WavePool pool2;
    public BulletEffectPool pool3;
    public ExplosionPool pool4;
    public AsteroidPool pool5;

    void Start()
    {
        pool1.Initialize();
        pool2.Initialize();
        pool3.Initialize();
        pool4.Initialize();
        pool5.Initialize();
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Bullet bullet = pool1.GetObject();
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        WaveEnemy enemy = pool2.GetObject();
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        BulletEffect hit = pool3.GetObject();
    }
    protected override void OnTest4(InputAction.CallbackContext context)
    {
        BulletEffect explosion = pool4.GetObject();
    }
    protected override void OnTest5(InputAction.CallbackContext context)
    {
        Asteroid ast = pool5.GetObject();
    }
}
