using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSensor : MonoBehaviour
{
    /// <summary>
    /// 슬라임이 내 트리거 안에 들어왔다고 알리는 델리게이트
    /// </summary>
    public Action<Slime> onSlimeEnter;

    /// <summary>
    /// 슬라임이 내 트리거 밖으로 나갔다로 알리는 델리게이트
    /// </summary>
    public Action<Slime> onSlimeExit;

    // 적 공격하기
    // 애니메이션에서 Player의 isAttack 값을 바꾼다 true false
    // AttackSensor에서 Player isAttack이 true이고 트리거가 활성화 되있으면 공격을 받는다.
    void OnTriggerEnter2D(Collider2D collision)
    {
        Slime slime = collision.GetComponent<Slime>();

        if(slime != null)
        {
            onSlimeEnter.Invoke(slime);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        Slime slime = collision.GetComponent<Slime>();

        if (slime != null)
        {
            onSlimeExit.Invoke(slime);
        }
    }
}
