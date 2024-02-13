using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBase : Waypoint_User
{
    /// <summary>
    /// 이동할때 실행될 델리게이트
    /// </summary>
    public Action<Vector3> onMove;

    protected override void OnMove()
    {
        base.OnMove();
        onMove?.Invoke(moveDelta);
    }
}