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

        // üũ�� ������Ʈ�� �θ���� ��� �˻�
        do
        {
            obj = target.GetComponent<IInteracable>();
            target = target.parent;
        } while (obj == null && target != null); // obj�� ã�Ѱų� �� �̻� �θ� ������ ���� ����

        if(obj != null)
        {
            onItemUse?.Invoke(obj); // IInteracable�� �ִ� ������Ʈ�� ��� �ߴٰ� �˸�
        }

    }

}
