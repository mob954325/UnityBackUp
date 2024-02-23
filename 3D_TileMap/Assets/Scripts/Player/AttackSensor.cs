using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSensor : MonoBehaviour
{
    /// <summary>
    /// �������� �� Ʈ���� �ȿ� ���Դٰ� �˸��� ��������Ʈ
    /// </summary>
    public Action<Slime> onSlimeEnter;

    /// <summary>
    /// �������� �� Ʈ���� ������ �����ٷ� �˸��� ��������Ʈ
    /// </summary>
    public Action<Slime> onSlimeExit;

    // �� �����ϱ�
    // �ִϸ��̼ǿ��� Player�� isAttack ���� �ٲ۴� true false
    // AttackSensor���� Player isAttack�� true�̰� Ʈ���Ű� Ȱ��ȭ �������� ������ �޴´�.
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
