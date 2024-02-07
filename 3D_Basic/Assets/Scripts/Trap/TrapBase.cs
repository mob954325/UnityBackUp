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
    /// ������ �ߵ��Ǹ� ó���� ������ �ִ� �Լ�
    /// </summary>
    protected virtual void OnActivateTrap()
    {

    }

    /// <summary>
    /// Ʈ���Ÿ� �ߵ���Ų ������Ʈ�� ���ϴ� ������ �ִ� �Լ�
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



    // 1. TrapSpike : ������ ���ð� �ö� �÷��̾ ���δ�.
    // 2. TrapPush : ������ ������ Ƣ�� �����鼭 �÷��̾ �о��.
    // 3. TrapFire : ������ �ٴڿ��� ���� �ö�� �÷��̾ ���δ�.
    // 4. TrapSlow : ������ �����ð����� �÷��̾��� �̵��ӵ��� ��������.
}
