using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUseChecker : MonoBehaviour
{
    public Action<IInteracable> onItemUse;

    void OnTriggerEnter(Collider other)
    {
        Transform target = other.transform;
        IInteracable obj = null;

        // 체크한 오브젝트의 부모까지 모두 검사
        do
        {
            obj = target.GetComponent<IInteracable>();
            target = target.parent;
        } while (obj == null && target != null); // obj를 찾앗거나 더 이상 부모가 없으면 루프 종료

        if(obj != null)
        {
            onItemUse?.Invoke(obj); // IInteracable이 있는 오브젝트를 사용 했다고 알림
        }

    }

}
