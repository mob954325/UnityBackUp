using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrapFire : TrapBase
{
    //ParticleSystem ps;
    //
    //protected override void Awake()
    //{
    //    base.Awake();
    //    
    //    Transform firePos = transform.GetChild(1);
    //    ps = firePos.GetChild(0).GetComponent<ParticleSystem>(); 
    //}
    //
    //protected override void OnActivateTrap()
    //{
    //    ps.Stop();
    //    StartCoroutine(GetFire());
    //}
    //
    //protected override void OnActivateTrapAction(Object obj)
    //{
    //    base.OnActivateTrapAction(obj);
    //
    //    obj.GetComponent<IAlive>().Die();
    //}
    //
    //IEnumerator GetFire()
    //{
    //    ps.Play();
    //    yield return new WaitForSeconds(1.5f);
    //    ps.Stop();
    //}
}
