using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBase : MonoBehaviour
{
/*    protected List<Object> alives;

    protected virtual void Awake()
    {
        alives = new List<Object>(5);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        Object alive = other.gameObject;

        if (alive != null)
        {
            alives.Add(alive);
        }

        OnActivateTrap();

        foreach (Object obj in alives)
        {
            OnActivateTrapAction(obj);
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        Object alive = other.gameObject;

        if (alive != null)
        {
            alives.Remove(alive);
        }
    }

    /// <summary>
    /// 함정이 발동되면 처리할 내용을 넣는 함수
    /// </summary>
    protected virtual void OnActivateTrap()
    {

    }

    /// <summary>
    /// 트리거를 발동시킨 오브젝트가 당하는 내용을 넣는 함수
    /// </summary>
    protected virtual void OnActivateTrapAction(Object obj)
    {

    }*/

    void OnTriggerEnter(Collider other)
    {
        OnTrapActivate(other.gameObject);
    }
    
    protected virtual void OnTrapActivate(GameObject gameObject)
    {
    }



    // 1. TrapSpike : 밟으면 가시가 올라가 플레이어를 죽인다.
    // 2. TrapPush : 밟으면 발판이 튀어 오르면서 플레이어를 밀어낸다.
    // 3. TrapFire : 밟으면 바닥에서 불이 올라와 플레이어를 죽인다.
    // 4. TrapSlow : 밟으면 일정시간동안 플레이어의 이동속도가 내려간다.
}
