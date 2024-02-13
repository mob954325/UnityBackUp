using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrapSpike : TrapBase
{
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
