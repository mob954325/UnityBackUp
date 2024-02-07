using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrapSpike : TrapBase
{
    /*    Animator animator;

        protected override void Awake()
        {
            base.Awake();

            animator = GetComponent<Animator>();
        }

        protected override void OnActivateTrap()
        {
            animator.SetTrigger("ActiveTrap");
        }

        protected override void OnActivateTrapAction(Object obj)
        {
            base.OnActivateTrapAction(obj);

            obj.GetComponent<IAlive>().Die();
        }*/

    Animator animator;
    readonly int ActivateHash = Animator.StringToHash("Activate");

    void Awake()
    {
        animator = GetComponent<Animator>();    
    }

    private void OnTriggerEnter(Collider target)
    {
        animator.SetTrigger(ActivateHash);
    }
}
