using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrapPush : TrapBase
{
    //Animator animator;
    //protected override void Awake()
    //{
    //    base.Awake();
    //
    //    animator = GetComponent<Animator>();
    //}
    //
    //protected override void OnActivateTrap()
    //{
    //    animator.SetTrigger("ActiveTrap");
    //}
    //
    //protected override void OnActivateTrapAction(Object obj)
    //{
    //    base.OnActivateTrapAction(obj);
    //
    //    obj.GetComponent<Rigidbody>().AddForce(Vector3.up * 5f + Vector3.back * 7f, ForceMode.Impulse);
    //}

    public float pushPower = 5.0f;
    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected override void OnTrapActivate(GameObject target)
    {
        animator.SetTrigger("Activate");
        Rigidbody rigid = target.GetComponent<Rigidbody>();

        if(rigid != null)
        {
            Vector3 dir = (transform.up + transform.forward).normalized;
            rigid.AddForce(pushPower * dir, ForceMode.Impulse);
        }
    }
}
