using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Bullet : TestBase
{
    public GameObject bulletPrefab;
    public PoolObjectType objectType;
    public float interval = 0.1f;

    Transform fireTransform;

    void Start()
    {
        fireTransform = transform.GetChild(0);
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Instantiate(bulletPrefab, fireTransform);
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        StartCoroutine(FireContinue());
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        Factory.Instance.GetObject(objectType, fireTransform.position);
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        StartCoroutine(FireContinue2());
    }

    IEnumerator FireContinue()
    {
        while(true)
        {
            Instantiate(bulletPrefab, fireTransform);
            yield return new WaitForSeconds(interval);
        }
    }

    IEnumerator FireContinue2()
    {
        while (true)
        {
            Factory.Instance.GetObject(objectType, fireTransform.position);
            yield return new WaitForSeconds(interval);
        }
    }
}
