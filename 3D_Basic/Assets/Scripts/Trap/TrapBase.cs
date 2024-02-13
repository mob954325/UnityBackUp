using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBase : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        OnTrapActivate(other.gameObject);
    }
    
    protected virtual void OnTrapActivate(GameObject gameObject)
    {
    }



    // 1. TrapSpike : ������ ���ð� �ö� �÷��̾ ���δ�.
    // 2. TrapPush : ������ ������ Ƣ�� �����鼭 �÷��̾ �о��.
    // 3. TrapFire : ������ �ٴڿ��� ���� �ö�� �÷��̾ ���δ�.
    // 4. TrapSlow : ������ �����ð����� �÷��̾��� �̵��ӵ��� ��������.
}
