using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSwitch : PlatformBase, IInteracable
{
    public bool CanUse => false; // 사용처 없음

    /// <summary>
    /// 플랫폼이 움직일 지 멈출지 결정하는 함수
    /// </summary>
    bool isMoving = false;

    void Start()
    {
        Target = targetWaypoints.GetNextWayPoint(); // 시작했을 때 플랫폼이 안움직이는 문제 해결용 (올라가자마자 도착하는 문제)
    }

    protected override void OnMove()
    {
        if (isMoving)
        {
            base.OnMove(); // 플레이어가 올라오면 이동하기
        }
    }

    protected override void OnArrived()
    {
        isMoving = false; // 도착하면 멈추기
        base.OnArrived();
    }

    public void Use()
    {
        isMoving = true; // 아이템 사용하면 움직이기
    }
}
