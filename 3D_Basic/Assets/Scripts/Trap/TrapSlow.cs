using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrapSlow : TrapBase
{
    //protected override void Awake()
    //{
    //    base.Awake();
    //}
    //
    //protected override void OnActivateTrap()
    //{
    //
    //}
    //
    //protected override void OnActivateTrapAction(Object obj)
    //{
    //    base.OnActivateTrapAction(obj);
    //
    //    Player player = obj.GetComponent<Player>();
    //
    //    if (player != null)
    //    {
    //        StartCoroutine(GetSlow(player));
    //    }
    //    else
    //        return;
    //}
    //
    //IEnumerator GetSlow(Player player)
    //{
    //    float preMoveSpeed = player.moveSpeed;
    //    player.moveSpeed /= 2;
    //    yield return new WaitForSeconds(2.5f);
    //    player.moveSpeed = preMoveSpeed;
    //}
}
